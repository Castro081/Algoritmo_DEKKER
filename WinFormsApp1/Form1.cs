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

            comboBoxVersion.Items.AddRange(new string[] { "Versión 1: Alternancia estricta", "Versión 2: Interbloqueo", "Versión 3: Colisión en región crítica", "Versión 4: Postergación indefinida" });
            comboBoxVersion.SelectedIndex = 0;
        }

        private void ThreadWorker(int threadIndex)
        {
            for (int i = 0; i < iterations; i++)
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
                    }
                }
            }

            lock (progressLock)
            {
                completedIterations++;
                if (completedIterations >= numThreads * iterations)
                {
                    UpdateExecutionTime();
                    UpdateMetrics();
                }
            }
        }

        // Versión 1: Alternancia estricta
        private void EnterCriticalSectionVersion1(int threadIndex, int iteration)
        {
            flags[threadIndex] = true;
            UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");

            while (turn != threadIndex)
            {
                UpdateThreadStatus(threadIndex, "Esperando turno");
                Thread.Sleep(10);
            }

            EnterCriticalSection(threadIndex, iteration);
            turn = 1 - threadIndex;
            flags[threadIndex] = false;
            UpdateThreadStatus(threadIndex, "Terminado");
        }

        // Versión 2: Interbloqueo
        private void EnterCriticalSectionVersion2(int threadIndex, int iteration)
        {
            flags[threadIndex] = true;
            UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");

            while (flags[1 - threadIndex])
            {
                UpdateThreadStatus(threadIndex, "Bloqueado (deadlock)");
                Thread.Sleep(10);
            }

            EnterCriticalSection(threadIndex, iteration);
            flags[threadIndex] = false;
            UpdateThreadStatus(threadIndex, "Terminado");
        }

        // Versión 3: Colisión en región crítica
        private void EnterCriticalSectionVersion3(int threadIndex, int iteration)
        {
            flags[threadIndex] = true;
            UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");

            Thread.Sleep(new Random().Next(0, 50));
            EnterCriticalSection(threadIndex, iteration);
            flags[threadIndex] = false;
            UpdateThreadStatus(threadIndex, "Terminado");
        }

        // Versión 4: Postergación indefinida
        private void EnterCriticalSectionVersion4(int threadIndex, int iteration)
        {
            flags[threadIndex] = true;
            UpdateThreadStatus(threadIndex, "Quiere entrar a la sección crítica");

            int attempts = 0;
            while (flags[1 - threadIndex])
            {
                attempts++;
                UpdateThreadStatus(threadIndex, $"Esperando (intento {attempts})");
                Thread.Sleep(10);
                if (threadIndex == 1)
                {
                    flags[0] = true; // Hilo 0 mantiene prioridad
                }
            }

            EnterCriticalSection(threadIndex, iteration);
            flags[threadIndex] = false;
            UpdateThreadStatus(threadIndex, "Terminado");
        }

        private void EnterCriticalSection(int threadIndex, int iteration)
        {
            DateTime criticalSectionStart = DateTime.Now;
            UpdateUI($"Hilo {threadIndex + 1} en sección crítica, iteración: {iteration}", GetTextBox(threadIndex), Color.Red);
            UpdateThreadStatus(threadIndex, "En sección crítica");

            lock (progressLock)
            {
                completedIterations++;
                criticalSectionCount[threadIndex]++;
            }

            int totalIterations = numThreads * iterations;
            UpdateGlobalProgress(completedIterations, totalIterations);
            UpdateProgressBar(completedIterations, totalIterations);

            Thread.Sleep(delay);

            criticalSectionTime += DateTime.Now - criticalSectionStart;

            UpdateUI($"Hilo {threadIndex + 1} en sección no crítica, iteración: {iteration}", GetTextBox(threadIndex), Color.Green);
            UpdateThreadStatus(threadIndex, "En sección no crítica");

            Thread.Sleep(delay);
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
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new Action(() =>
                {
                    textBox.Text = message;
                    textBox.BackColor = color;
                    richTextBox1.AppendText(message + Environment.NewLine);
                }));
            }
            else
            {
                textBox.Text = message;
                textBox.BackColor = color;
                richTextBox1.AppendText(message + Environment.NewLine);
            }
        }

        private void UpdateThreadStatus(int threadIndex, string status)
        {
            if (listViewThreads.InvokeRequired)
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
            else
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

        private void UpdateGlobalProgress(int completedIterations, int totalIterations)
        {
            if (labelProgress.InvokeRequired)
            {
                labelProgress.Invoke(new Action(() =>
                {
                    labelProgress.Text = $"Progreso: {completedIterations} / {totalIterations}";
                }));
            }
            else
            {
                labelProgress.Text = $"Progreso: {completedIterations} / {totalIterations}";
            }
        }

        private void UpdateProgressBar(int completedIterations, int totalIterations)
        {
            int totalProgress = (completedIterations * 100) / totalIterations;
            if (totalProgress > 100) totalProgress = 100;

            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => progressBar1.Value = totalProgress));
            }
            else
            {
                progressBar1.Value = totalProgress;
            }
        }

        private void UpdateExecutionTime()
        {
            if (labelExecutionTime.InvokeRequired)
            {
                labelExecutionTime.Invoke(new Action(() =>
                {
                    TimeSpan executionTime = DateTime.Now - startTime;
                    labelExecutionTime.Text = $"{executionTime.TotalSeconds:F2} segundos";
                }));
            }
            else
            {
                TimeSpan executionTime = DateTime.Now - startTime;
                labelExecutionTime.Text = $"{executionTime.TotalSeconds:F2} segundos";
            }
        }

        private void UpdateMetrics()
        {
            if (labelAvgTimePerIteration.InvokeRequired)
            {
                labelAvgTimePerIteration.Invoke(new Action(() =>
                {
                    TimeSpan executionTime = DateTime.Now - startTime;
                    double avgTimePerIteration = executionTime.TotalSeconds / (numThreads * iterations);
                    labelAvgTimePerIteration.Text = $"{avgTimePerIteration:F2} segundos";
                }));
            }
            else
            {
                TimeSpan executionTime = DateTime.Now - startTime;
                double avgTimePerIteration = executionTime.TotalSeconds / (numThreads * iterations);
                labelAvgTimePerIteration.Text = $"{avgTimePerIteration:F2} segundos";
            }

            if (labelCriticalSectionTime.InvokeRequired)
            {
                labelCriticalSectionTime.Invoke(new Action(() =>
                {
                    labelCriticalSectionTime.Text = $"{criticalSectionTime.TotalSeconds:F2} segundos";
                }));
            }
            else
            {
                labelCriticalSectionTime.Text = $"{criticalSectionTime.TotalSeconds:F2} segundos";
            }

            if (labelNonCriticalSectionTime.InvokeRequired)
            {
                labelNonCriticalSectionTime.Invoke(new Action(() =>
                {
                    labelNonCriticalSectionTime.Text = $"{nonCriticalSectionTime.TotalSeconds:F2} segundos";
                }));
            }
            else
            {
                labelNonCriticalSectionTime.Text = $"{nonCriticalSectionTime.TotalSeconds:F2} segundos";
            }

            if (labelCriticalSectionCount.InvokeRequired)
            {
                labelCriticalSectionCount.Invoke(new Action(() =>
                {
                    labelCriticalSectionCount.Text = $"Hilo 1: {criticalSectionCount[0]}, Hilo 2: {criticalSectionCount[1]}";
                }));
            }
            else
            {
                labelCriticalSectionCount.Text = $"Hilo 1: {criticalSectionCount[0]}, Hilo 2: {criticalSectionCount[1]}";
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

                richTextBox1.Clear();
                listViewThreads.Items.Clear();
                progressBar1.Value = 0;
                completedIterations = 0;
                startTime = DateTime.Now;
                criticalSectionTime = TimeSpan.Zero;
                nonCriticalSectionTime = TimeSpan.Zero;

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
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            delay = 1000 / trackBarSpeed.Value;
        }
    }
}