using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private static bool[] flags;
        private static int turn = 0;
        private int iterations = 10;
        private int delay = 500;
        private int numThreads = 2;
        private Thread[] threads;
        private int completedIterations = 0;
        private static object progressLock = new object();
        private DateTime startTime;
        private int[] criticalSectionCount;
        private TimeSpan criticalSectionTime;
        private TimeSpan nonCriticalSectionTime;
        private int version = 1;
        private volatile bool isRunning = true;

        public Form1()
        {
            InitializeComponent();
            flags = new bool[numThreads];
            criticalSectionCount = new int[numThreads];
            listViewThreads.Columns.Add("Hilo", 100);
            listViewThreads.Columns.Add("Estado", 150);
            listViewThreads.Columns.Add("Prioridad", 100);
            listViewThreads.View = View.Details;

            numericUpDownThreads.Minimum = 1;
            numericUpDownThreads.Maximum = 2;

            comboBoxVersion.Items.AddRange(new string[] {
                "Versión 1: Alternancia estricta",
                "Versión 2: Interbloqueo",
                "Versión 3: Colisión en región crítica",
                "Versión 4: Postergación indefinida",
                "Versión 5: Dekker Correcto" });
            comboBoxVersion.SelectedIndex = 0;
        }

        private void ThreadWorker(int threadIndex)
        {
            try
            {
                for (int i = 0; i < iterations && isRunning; i++)
                {
                    if (numThreads == 1)
                    {
                        EnterCriticalSection(threadIndex, i);
                    }
                    else
                    {
                        switch (version)
                        {
                            case 1:
                                EnterCriticalSectionVersion1(threadIndex, i);
                                break;
                            case 2:
                                EnterCriticalSectionVersion2(threadIndex, i);
                                break;
                            case 3:
                                EnterCriticalSectionVersion3(threadIndex, i);
                                break;
                            case 4:
                                EnterCriticalSectionVersion4(threadIndex, i);
                                break;
                            case 5:
                                EnterCriticalSectionVersion5(threadIndex, i);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en ThreadWorker (Hilo {threadIndex + 1}): {ex.Message}");
            }
        }

        // Versión 1: Alternancia estricta
        private void EnterCriticalSectionVersion1(int threadIndex, int iteration)
        {
            try
            {
                flags[threadIndex] = true;
                UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");
                LogHistory(threadIndex, "Quiere entrar a la sección crítica", iteration, "El hilo solicita acceso a la sección crítica.");

                while (turn != threadIndex)
                {
                    UpdateThreadStatus(threadIndex, "Esperando turno");
                    LogHistory(threadIndex, "Esperando turno", iteration, "El hilo está esperando su turno según la alternancia estricta.");
                    Thread.Sleep(10);
                }

                EnterCriticalSection(threadIndex, iteration);
                turn = 1 - threadIndex;
                flags[threadIndex] = false;
                UpdateThreadStatus(threadIndex, "Terminado");
                LogHistory(threadIndex, "Terminado", iteration, "El hilo ha completado su iteración y libera la sección crítica.");

                lock (progressLock)
                {
                    if (completedIterations < numThreads * iterations)
                    {
                        completedIterations++;
                        UpdateGlobalProgress(completedIterations, numThreads * iterations);
                        UpdateProgressBar(completedIterations, numThreads * iterations);

                        if (completedIterations == numThreads * iterations)
                        {
                            UpdateExecutionTime();
                            UpdateMetrics();
                            LogHistorySummary();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en EnterCriticalSectionVersion1 (Hilo {threadIndex + 1}, Iteración {iteration}): {ex.Message}");
            }
        }

        // Versión 2: Interbloqueo
        private void EnterCriticalSectionVersion2(int threadIndex, int iteration)
        {
            try
            {
                flags[threadIndex] = true;
                UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");
                UpdateUI($"Hilo {threadIndex + 1} en sección crítica, iteración: {iteration} - Quiere entrar", GetTextBox(threadIndex), Color.Yellow);
                LogHistory(threadIndex, "Quiere entrar a la sección crítica", iteration, "El hilo solicita acceso a la sección crítica.");

                bool loggedDeadlock = false;
                while (flags[1 - threadIndex])
                {
                    UpdateThreadStatus(threadIndex, "Bloqueado (deadlock)");
                    UpdateUI($"Hilo {threadIndex + 1} en sección crítica, iteración: {iteration} - Bloqueado", GetTextBox(threadIndex), Color.Orange);
                    if (!loggedDeadlock)
                    {
                        LogHistory(threadIndex, "Bloqueado (deadlock)", iteration, "El hilo está en un estado de interbloqueo porque el otro hilo retiene el acceso.");
                        loggedDeadlock = true;
                    }
                    Thread.Sleep(10);
                }

                EnterCriticalSection(threadIndex, iteration);
                flags[threadIndex] = false;
                UpdateThreadStatus(threadIndex, "Terminado");
                LogHistory(threadIndex, "Terminado", iteration, "El hilo ha salido del interbloqueo y completado su iteración.");

                lock (progressLock)
                {
                    if (completedIterations < numThreads * iterations)
                    {
                        completedIterations++;
                        UpdateGlobalProgress(completedIterations, numThreads * iterations);
                        UpdateProgressBar(completedIterations, numThreads * iterations);

                        if (completedIterations == numThreads * iterations)
                        {
                            UpdateExecutionTime();
                            UpdateMetrics();
                            LogHistorySummary();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en EnterCriticalSectionVersion2 (Hilo {threadIndex + 1}, Iteración {iteration}): {ex.Message}");
            }
        }

        // Versión 3: Colisión en región crítica
        private void EnterCriticalSectionVersion3(int threadIndex, int iteration)
        {
            try
            {
                flags[threadIndex] = true;
                UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");
                LogHistory(threadIndex, "Quiere entrar a la sección crítica", iteration, "El hilo solicita acceso a la sección crítica.");

                Thread.Sleep(new Random().Next(0, 50));
                EnterCriticalSection(threadIndex, iteration);
                flags[threadIndex] = false;
                UpdateThreadStatus(threadIndex, "Terminado");
                LogHistory(threadIndex, "Terminado", iteration, "El hilo ha completado su iteración, pudiendo haber colisionado con otro hilo.");

                lock (progressLock)
                {
                    if (completedIterations < numThreads * iterations)
                    {
                        completedIterations++;
                        UpdateGlobalProgress(completedIterations, numThreads * iterations);
                        UpdateProgressBar(completedIterations, numThreads * iterations);

                        if (completedIterations == numThreads * iterations)
                        {
                            UpdateExecutionTime();
                            UpdateMetrics();
                            LogHistorySummary();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en EnterCriticalSectionVersion3 (Hilo {threadIndex + 1}, Iteración {iteration}): {ex.Message}");
            }
        }

        // Versión 4: Postergación indefinida
        private void EnterCriticalSectionVersion4(int threadIndex, int iteration)
        {
            try
            {
                flags[threadIndex] = true;
                UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");
                LogHistory(threadIndex, "Quiere entrar a la sección crítica", iteration, "El hilo solicita acceso a la sección crítica.");

                int attempts = 0;
                while (flags[1 - threadIndex] && isRunning)
                {
                    attempts++;
                    UpdateThreadStatus(threadIndex, $"Esperando (intento {attempts})");
                    LogHistory(threadIndex, $"Esperando (intento {attempts})", iteration, "El hilo está esperando porque el otro hilo retiene el acceso.");
                    Thread.Sleep(10);
                    if (threadIndex == 1 && attempts % 5 == 0)
                    {
                        flags[0] = false;
                        Thread.Sleep(50);
                    }
                }

                EnterCriticalSection(threadIndex, iteration);
                flags[threadIndex] = false;
                UpdateThreadStatus(threadIndex, "Terminado");
                LogHistory(threadIndex, "Terminado", iteration, "El hilo ha superado la postergación y completado su iteración.");

                lock (progressLock)
                {
                    if (completedIterations < numThreads * iterations)
                    {
                        completedIterations++;
                        UpdateGlobalProgress(completedIterations, numThreads * iterations);
                        UpdateProgressBar(completedIterations, numThreads * iterations);

                        if (completedIterations == numThreads * iterations)
                        {
                            UpdateExecutionTime();
                            UpdateMetrics();
                            LogHistorySummary();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en EnterCriticalSectionVersion4 (Hilo {threadIndex + 1}, Iteración {iteration}): {ex.Message}");
            }
        }

        // Versión 5: Dekker Correcto
        private void EnterCriticalSectionVersion5(int threadIndex, int iteration)
        {
            try
            {
                int otherThread = 1 - threadIndex;
                flags[threadIndex] = true;
                UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");
                LogHistory(threadIndex, "Quiere entrar a la sección crítica", iteration, "El hilo solicita acceso a la sección crítica.");

                while (flags[otherThread])
                {
                    if (turn == otherThread)
                    {
                        flags[threadIndex] = false;
                        UpdateThreadStatus(threadIndex, "Esperando turno");
                        LogHistory(threadIndex, "Esperando turno", iteration, "El hilo cede el turno al otro hilo.");
                        while (turn == otherThread)
                        {
                            Thread.Sleep(10);
                        }
                        flags[threadIndex] = true;
                        UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");
                        LogHistory(threadIndex, "Quiere entrar a la sección crítica", iteration, "El hilo vuelve a solicitar acceso después de esperar.");
                    }
                }

                EnterCriticalSection(threadIndex, iteration);
                turn = otherThread;
                flags[threadIndex] = false;
                UpdateThreadStatus(threadIndex, "Terminado");
                LogHistory(threadIndex, "Terminado", iteration, "El hilo ha completado su iteración.");

                lock (progressLock)
                {
                    if (completedIterations < numThreads * iterations)
                    {
                        completedIterations++;
                        UpdateGlobalProgress(completedIterations, numThreads * iterations);
                        UpdateProgressBar(completedIterations, numThreads * iterations);

                        if (completedIterations == numThreads * iterations)
                        {
                            UpdateExecutionTime();
                            UpdateMetrics();
                            LogHistorySummary();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en EnterCriticalSectionVersion5 (Hilo {threadIndex + 1}, Iteración {iteration}): {ex.Message}");
            }
        }

        private void EnterCriticalSection(int threadIndex, int iteration)
        {
            try
            {
                DateTime criticalSectionStart = DateTime.Now;
                UpdateUI($"Hilo {threadIndex + 1} en sección crítica, iteración: {iteration}", GetTextBox(threadIndex), Color.Red);
                UpdateThreadStatus(threadIndex, "En sección crítica");
                LogHistory(threadIndex, "En sección crítica", iteration, "El hilo está ejecutando su tarea en la sección crítica.");

                lock (progressLock)
                {
                    if (threadIndex < criticalSectionCount.Length)
                    {
                        criticalSectionCount[threadIndex]++;
                    }
                }

                Thread.Sleep(delay);

                criticalSectionTime += DateTime.Now - criticalSectionStart;

                DateTime nonCriticalSectionStart = DateTime.Now;
                UpdateUI($"Hilo {threadIndex + 1} en sección no crítica, iteración: {iteration}", GetTextBox(threadIndex), Color.Green);
                UpdateThreadStatus(threadIndex, "En sección no crítica");
                LogHistory(threadIndex, "En sección no crítica", iteration, "El hilo está ejecutando su tarea fuera de la sección crítica.");

                Thread.Sleep(delay);

                nonCriticalSectionTime += DateTime.Now - nonCriticalSectionStart;
            }
            catch (Exception ex)
            {
                LogError($"Error en EnterCriticalSection (Hilo {threadIndex + 1}, Iteración {iteration}): {ex.Message}");
            }
        }

        private TextBox GetTextBox(int threadIndex)
        {
            switch (threadIndex)
            {
                case 0: return textBox1;
                case 1: return textBox2;
                default: return textBox1;
            }
        }

        private void UpdateUI(string message, TextBox textBox, Color color)
        {
            try
            {
                if (textBox.InvokeRequired && textBox.IsHandleCreated)
                {
                    textBox.Invoke(new Action(() =>
                    {
                        textBox.Text = message;
                        textBox.BackColor = color;
                        richTextBox1.AppendText(message + Environment.NewLine);
                    }));
                }
                else if (textBox.IsHandleCreated)
                {
                    textBox.Text = message;
                    textBox.BackColor = color;
                    richTextBox1.AppendText(message + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en UpdateUI: {ex.Message}");
            }
        }

        private void UpdateThreadStatus(int threadIndex, string status)
        {
            try
            {
                if (listViewThreads.InvokeRequired && listViewThreads.IsHandleCreated)
                {
                    listViewThreads.Invoke(new Action(() =>
                    {
                        ListViewItem item = listViewThreads.Items.Cast<ListViewItem>()
                            .FirstOrDefault(x => x.Text == $"Hilo {threadIndex + 1}");

                        if (item == null)
                        {
                            item = new ListViewItem($"Hilo {threadIndex + 1}");
                            item.SubItems.Add(status);
                            item.SubItems.Add((threadIndex + 1).ToString());
                            listViewThreads.Items.Add(item);
                        }
                        else
                        {
                            item.SubItems[1].Text = status;
                        }
                    }));
                }
                else if (listViewThreads.IsHandleCreated)
                {
                    ListViewItem item = listViewThreads.Items.Cast<ListViewItem>()
                        .FirstOrDefault(x => x.Text == $"Hilo {threadIndex + 1}");

                    if (item == null)
                    {
                        item = new ListViewItem($"Hilo {threadIndex + 1}");
                        item.SubItems.Add(status);
                        item.SubItems.Add((threadIndex + 1).ToString());
                        listViewThreads.Items.Add(item);
                    }
                    else
                    {
                        item.SubItems[1].Text = status;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en UpdateThreadStatus (Hilo {threadIndex + 1}): {ex.Message}");
            }
        }

        private void UpdateGlobalProgress(int completedIterations, int totalIterations)
        {
            try
            {
                int cappedCompletedIterations = Math.Min(completedIterations, totalIterations);
                if (labelProgress.InvokeRequired && labelProgress.IsHandleCreated)
                {
                    labelProgress.Invoke(new Action(() =>
                    {
                        labelProgress.Text = $"Progreso: {cappedCompletedIterations} / {totalIterations}";
                    }));
                }
                else if (labelProgress.IsHandleCreated)
                {
                    labelProgress.Text = $"Progreso: {cappedCompletedIterations} / {totalIterations}";
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en UpdateGlobalProgress: {ex.Message}");
            }
        }

        private void UpdateProgressBar(int completedIterations, int totalIterations)
        {
            try
            {
                int cappedCompletedIterations = Math.Min(completedIterations, totalIterations);
                int totalProgress = (cappedCompletedIterations * 100) / totalIterations;
                if (totalProgress > 100) totalProgress = 100;

                if (progressBar1.InvokeRequired && progressBar1.IsHandleCreated)
                {
                    progressBar1.Invoke(new Action(() => progressBar1.Value = totalProgress));
                }
                else if (progressBar1.IsHandleCreated)
                {
                    progressBar1.Value = totalProgress;
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en UpdateProgressBar: {ex.Message}");
            }
        }

        private void UpdateExecutionTime()
        {
            try
            {
                if (labelExecutionTime.InvokeRequired && labelExecutionTime.IsHandleCreated)
                {
                    labelExecutionTime.Invoke(new Action(() =>
                    {
                        TimeSpan executionTime = DateTime.Now - startTime;
                        labelExecutionTime.Text = $"{executionTime.TotalSeconds:F2} segundos";
                    }));
                }
                else if (labelExecutionTime.IsHandleCreated)
                {
                    TimeSpan executionTime = DateTime.Now - startTime;
                    labelExecutionTime.Text = $"{executionTime.TotalSeconds:F2} segundos";
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en UpdateExecutionTime: {ex.Message}");
            }
        }

        private void UpdateMetrics()
        {
            try
            {
                if (labelAvgTimePerIteration.InvokeRequired && labelAvgTimePerIteration.IsHandleCreated)
                {
                    labelAvgTimePerIteration.Invoke(new Action(() =>
                    {
                        TimeSpan executionTime = DateTime.Now - startTime;
                        double avgTimePerIteration = executionTime.TotalSeconds / (numThreads * iterations);
                        labelAvgTimePerIteration.Text = $"{avgTimePerIteration:F2} segundos";
                    }));
                }
                else if (labelAvgTimePerIteration.IsHandleCreated)
                {
                    TimeSpan executionTime = DateTime.Now - startTime;
                    double avgTimePerIteration = executionTime.TotalSeconds / (numThreads * iterations);
                    labelAvgTimePerIteration.Text = $"{avgTimePerIteration:F2} segundos";
                }

                if (labelCriticalSectionTime.InvokeRequired && labelCriticalSectionTime.IsHandleCreated)
                {
                    labelCriticalSectionTime.Invoke(new Action(() =>
                    {
                        labelCriticalSectionTime.Text = $"{criticalSectionTime.TotalSeconds:F2} segundos";
                    }));
                }
                else if (labelCriticalSectionTime.IsHandleCreated)
                {
                    labelCriticalSectionTime.Text = $"{criticalSectionTime.TotalSeconds:F2} segundos";
                }

                if (labelNonCriticalSectionTime.InvokeRequired && labelNonCriticalSectionTime.IsHandleCreated)
                {
                    labelNonCriticalSectionTime.Invoke(new Action(() =>
                    {
                        labelNonCriticalSectionTime.Text = $"{nonCriticalSectionTime.TotalSeconds:F2} segundos";
                    }));
                }
                else if (labelNonCriticalSectionTime.IsHandleCreated)
                {
                    labelNonCriticalSectionTime.Text = $"{nonCriticalSectionTime.TotalSeconds:F2} segundos";
                }

                if (labelCriticalSectionCount.InvokeRequired && labelCriticalSectionCount.IsHandleCreated)
                {
                    labelCriticalSectionCount.Invoke(new Action(() =>
                    {
                        string countText = "Hilo 1: 0, Hilo 2: 0";
                        if (criticalSectionCount.Length >= 2)
                        {
                            countText = $"Hilo 1: {criticalSectionCount[0]}, Hilo 2: {criticalSectionCount[1]}";
                        }
                        else if (criticalSectionCount.Length == 1)
                        {
                            countText = $"Hilo 1: {criticalSectionCount[0]}, Hilo 2: 0";
                        }
                        labelCriticalSectionCount.Text = countText;
                    }));
                }
                else if (labelCriticalSectionCount.IsHandleCreated)
                {
                    string countText = "Hilo 1: 0, Hilo 2: 0";
                    if (criticalSectionCount.Length >= 2)
                    {
                        countText = $"Hilo 1: {criticalSectionCount[0]}, Hilo 2: {criticalSectionCount[1]}";
                    }
                    else if (criticalSectionCount.Length == 1)
                    {
                        countText = $"Hilo 1: {criticalSectionCount[0]}, Hilo 2: 0";
                    }
                    labelCriticalSectionCount.Text = countText;
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en UpdateMetrics: {ex.Message}");
            }
        }

        private void LogHistory(int threadIndex, string status, int iteration, string description)
        {
            try
            {
                string logEntry = $"Iteración {iteration + 1} - Hilo {threadIndex + 1}:\n" +
                                 $"  - Estado: {status}\n" +
                                 $"  - Descripción: {description}\n";
                if (richTextBox1.InvokeRequired && richTextBox1.IsHandleCreated)
                {
                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(logEntry + Environment.NewLine)));
                }
                else if (richTextBox1.IsHandleCreated)
                {
                    richTextBox1.AppendText(logEntry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en LogHistory (Hilo {threadIndex + 1}, Iteración {iteration}): {ex.Message}");
            }
        }

        private void LogHistorySummary()
        {
            try
            {
                TimeSpan executionTime = DateTime.Now - startTime;
                string summary = $"\n--- Resumen de Ejecución ---\n" +
                                $"Total de iteraciones completadas: {numThreads * iterations}\n" +
                                $"Tiempo total de ejecución: {executionTime.TotalSeconds:F2} segundos\n" +
                                $"Entradas a sección crítica: Hilo 1: {criticalSectionCount[0]}, Hilo 2: {criticalSectionCount[1]}\n";
                if (richTextBox1.InvokeRequired && richTextBox1.IsHandleCreated)
                {
                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(summary + Environment.NewLine)));
                }
                else if (richTextBox1.IsHandleCreated)
                {
                    richTextBox1.AppendText(summary + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en LogHistorySummary: {ex.Message}");
            }
        }

        private void LogError(string errorMessage)
        {
            try
            {
                string logEntry = $"[ERROR] {DateTime.Now:HH:mm:ss} - {errorMessage}";
                if (richTextBox1.InvokeRequired && richTextBox1.IsHandleCreated)
                {
                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(logEntry + Environment.NewLine)));
                }
                else if (richTextBox1.IsHandleCreated)
                {
                    richTextBox1.AppendText(logEntry + Environment.NewLine);
                }
            }
            catch
            {
              
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (numericUpDownThreads.Value <= 0 || numericUpDownThreads.Value > 2)
                {
                    MessageBox.Show("El número de hilos debe ser 1 o 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (numericUpDown1.Value <= 0)
                {
                    MessageBox.Show("El número de iteraciones debe ser mayor que 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mostrar mensaje con la versión seleccionada
                string selectedVersion = comboBoxVersion.SelectedItem.ToString();
                MessageBox.Show($"Iniciando ejecución con: {selectedVersion}", "Versión Seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                richTextBox1.Clear();
                listViewThreads.Items.Clear();
                progressBar1.Value = 0;
                completedIterations = 0;
                startTime = DateTime.Now;
                criticalSectionTime = TimeSpan.Zero;
                nonCriticalSectionTime = TimeSpan.Zero;
                isRunning = true;

                iterations = (int)numericUpDown1.Value;
                delay = 1000 / trackBarSpeed.Value;
                numThreads = (int)numericUpDownThreads.Value;
                version = comboBoxVersion.SelectedIndex + 1;

                flags = new bool[numThreads];
                criticalSectionCount = new int[numThreads];
                threads = new Thread[numThreads];

                for (int i = 0; i < numThreads; i++)
                {
                    int threadIndex = i;
                    threads[i] = new Thread(() => ThreadWorker(threadIndex));
                    threads[i].Start();
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en buttonStart_Click: {ex.Message}");
                MessageBox.Show($"Se produjo un error al iniciar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            try
            {
                isRunning = false;
                richTextBox1.Clear();
                listViewThreads.Items.Clear();
                progressBar1.Value = 0;
                labelProgress.Text = "Progreso: 0 / 0";
                labelExecutionTime.Text = "0.00 segundos";
                labelAvgTimePerIteration.Text = "0.00 segundos";
                labelCriticalSectionTime.Text = "0.00 segundos";
                labelNonCriticalSectionTime.Text = "0.00 segundos";
                labelCriticalSectionCount.Text = "Hilo 1: 0, Hilo 2: 0";
                textBox1.Clear();
                textBox2.Clear();
                textBox1.BackColor = SystemColors.Window;
                textBox2.BackColor = SystemColors.Window;
                numericUpDown1.Value = 10;
                numericUpDownThreads.Value = 1;
            }
            catch (Exception ex)
            {
                LogError($"Error en buttonClear_Click: {ex.Message}");
                MessageBox.Show($"Se produjo un error al limpiar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveLog_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Archivo de texto|*.txt",
                    Title = "Guardar Log"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, richTextBox1.Text);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error en buttonSaveLog_Click: {ex.Message}");
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            try
            {
                delay = 1000 / trackBarSpeed.Value;
            }
            catch (Exception ex)
            {
                LogError($"Error en trackBarSpeed_Scroll: {ex.Message}");
            }
        }
    }
}