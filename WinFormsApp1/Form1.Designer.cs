namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            richTextBox1 = new RichTextBox();
            progressBar1 = new ProgressBar();
            numericUpDown1 = new NumericUpDown();
            buttonStart = new Button();
            trackBarSpeed = new TrackBar();
            buttonSaveLog = new Button();
            numericUpDownThreads = new NumericUpDown();
            labelProgress = new Label();
            buttonClear = new Button();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            Historial = new Label();
            listViewThreads = new ListView();
            label7 = new Label();
            labelExecutionTime = new Label();
            labelAvgTimePerIteration = new Label();
            label9 = new Label();
            labelCriticalSectionTime = new Label();
            label11 = new Label();
            labelNonCriticalSectionTime = new Label();
            label13 = new Label();
            labelCriticalSectionCount = new Label();
            label15 = new Label();
            label8 = new Label();
            comboBoxVersion = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownThreads).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 12);
            label1.Name = "label1";
            label1.Size = new Size(29, 15);
            label1.TabIndex = 1;
            label1.Text = "Hilo";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 87);
            label2.Name = "label2";
            label2.Size = new Size(29, 15);
            label2.TabIndex = 2;
            label2.Text = "Hilo";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.Black;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(69, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(290, 16);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.Black;
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.ForeColor = Color.White;
            textBox2.Location = new Point(69, 79);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(290, 16);
            textBox2.TabIndex = 4;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.Gray;
            richTextBox1.ForeColor = Color.White;
            richTextBox1.Location = new Point(704, 51);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(362, 415);
            richTextBox1.TabIndex = 5;
            richTextBox1.Text = "";
            // 
            // progressBar1
            // 
            progressBar1.BackColor = Color.Black;
            progressBar1.Location = new Point(82, 216);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(225, 23);
            progressBar1.TabIndex = 6;
            // 
            // numericUpDown1
            // 
            numericUpDown1.BackColor = Color.Black;
            numericUpDown1.ForeColor = Color.White;
            numericUpDown1.Location = new Point(194, 150);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(55, 23);
            numericUpDown1.TabIndex = 7;
            // 
            // buttonStart
            // 
            buttonStart.BackColor = Color.Black;
            buttonStart.ForeColor = Color.White;
            buttonStart.Location = new Point(16, 294);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(75, 23);
            buttonStart.TabIndex = 8;
            buttonStart.Text = "Iniciar";
            buttonStart.UseVisualStyleBackColor = false;
            buttonStart.Click += buttonStart_Click;
            // 
            // trackBarSpeed
            // 
            trackBarSpeed.BackColor = Color.Black;
            trackBarSpeed.Location = new Point(16, 150);
            trackBarSpeed.Minimum = 1;
            trackBarSpeed.Name = "trackBarSpeed";
            trackBarSpeed.Size = new Size(156, 45);
            trackBarSpeed.TabIndex = 10;
            trackBarSpeed.Value = 5;
            trackBarSpeed.Scroll += trackBarSpeed_Scroll;
            // 
            // buttonSaveLog
            // 
            buttonSaveLog.BackColor = Color.Black;
            buttonSaveLog.ForeColor = Color.White;
            buttonSaveLog.Location = new Point(284, 294);
            buttonSaveLog.Name = "buttonSaveLog";
            buttonSaveLog.Size = new Size(75, 23);
            buttonSaveLog.TabIndex = 11;
            buttonSaveLog.Text = "Guardar";
            buttonSaveLog.UseVisualStyleBackColor = false;
            buttonSaveLog.Click += buttonSaveLog_Click;
            // 
            // numericUpDownThreads
            // 
            numericUpDownThreads.BackColor = Color.Black;
            numericUpDownThreads.ForeColor = Color.White;
            numericUpDownThreads.Location = new Point(303, 150);
            numericUpDownThreads.Name = "numericUpDownThreads";
            numericUpDownThreads.Size = new Size(55, 23);
            numericUpDownThreads.TabIndex = 14;
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.Location = new Point(586, 440);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(54, 15);
            labelProgress.TabIndex = 15;
            labelProgress.Text = "Progreso";
            // 
            // buttonClear
            // 
            buttonClear.BackColor = Color.Black;
            buttonClear.ForeColor = Color.White;
            buttonClear.Location = new Point(155, 294);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(75, 23);
            buttonClear.TabIndex = 16;
            buttonClear.Text = "Limpiar";
            buttonClear.UseVisualStyleBackColor = false;
            buttonClear.Click += buttonClear_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 123);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 17;
            label3.Text = "Velocidad";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(194, 123);
            label4.Name = "label4";
            label4.Size = new Size(64, 15);
            label4.TabIndex = 18;
            label4.Text = "Iteraciones";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(172, 198);
            label5.Name = "label5";
            label5.Size = new Size(49, 15);
            label5.TabIndex = 19;
            label5.Text = "Proceso";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(303, 123);
            label6.Name = "label6";
            label6.Size = new Size(34, 15);
            label6.TabIndex = 20;
            label6.Text = "Hilos";
            // 
            // Historial
            // 
            Historial.AutoSize = true;
            Historial.Location = new Point(861, 20);
            Historial.Name = "Historial";
            Historial.Size = new Size(51, 15);
            Historial.TabIndex = 21;
            Historial.Text = "Historial";
            // 
            // listViewThreads
            // 
            listViewThreads.BackColor = Color.Gray;
            listViewThreads.ForeColor = Color.White;
            listViewThreads.Location = new Point(34, 352);
            listViewThreads.Name = "listViewThreads";
            listViewThreads.Size = new Size(339, 114);
            listViewThreads.TabIndex = 22;
            listViewThreads.UseCompatibleStateImageBehavior = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(482, 35);
            label7.Name = "label7";
            label7.Size = new Size(121, 15);
            label7.TabIndex = 23;
            label7.Text = "Tiempo de Ejecución:";
            // 
            // labelExecutionTime
            // 
            labelExecutionTime.AutoSize = true;
            labelExecutionTime.Location = new Point(516, 64);
            labelExecutionTime.Name = "labelExecutionTime";
            labelExecutionTime.Size = new Size(16, 15);
            labelExecutionTime.TabIndex = 24;
            labelExecutionTime.Text = "...";
            // 
            // labelAvgTimePerIteration
            // 
            labelAvgTimePerIteration.AutoSize = true;
            labelAvgTimePerIteration.Location = new Point(516, 123);
            labelAvgTimePerIteration.Name = "labelAvgTimePerIteration";
            labelAvgTimePerIteration.Size = new Size(16, 15);
            labelAvgTimePerIteration.TabIndex = 26;
            labelAvgTimePerIteration.Text = "...";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(457, 96);
            label9.Name = "label9";
            label9.Size = new Size(176, 15);
            label9.TabIndex = 25;
            label9.Text = "Tiempo promedio por iteración:";
            // 
            // labelCriticalSectionTime
            // 
            labelCriticalSectionTime.AutoSize = true;
            labelCriticalSectionTime.Location = new Point(516, 195);
            labelCriticalSectionTime.Name = "labelCriticalSectionTime";
            labelCriticalSectionTime.Size = new Size(16, 15);
            labelCriticalSectionTime.TabIndex = 28;
            labelCriticalSectionTime.Text = "...";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(467, 158);
            label11.Name = "label11";
            label11.Size = new Size(145, 15);
            label11.TabIndex = 27;
            label11.Text = "Tiempo en sección critica:";
            // 
            // labelNonCriticalSectionTime
            // 
            labelNonCriticalSectionTime.AutoSize = true;
            labelNonCriticalSectionTime.Location = new Point(516, 259);
            labelNonCriticalSectionTime.Name = "labelNonCriticalSectionTime";
            labelNonCriticalSectionTime.Size = new Size(16, 15);
            labelNonCriticalSectionTime.TabIndex = 30;
            labelNonCriticalSectionTime.Text = "...";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(457, 225);
            label13.Name = "label13";
            label13.Size = new Size(162, 15);
            label13.TabIndex = 29;
            label13.Text = "Tiempo en sección no crítica:";
            // 
            // labelCriticalSectionCount
            // 
            labelCriticalSectionCount.AutoSize = true;
            labelCriticalSectionCount.Location = new Point(482, 331);
            labelCriticalSectionCount.Name = "labelCriticalSectionCount";
            labelCriticalSectionCount.Size = new Size(16, 15);
            labelCriticalSectionCount.TabIndex = 32;
            labelCriticalSectionCount.Text = "...";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(395, 298);
            label15.Name = "label15";
            label15.Size = new Size(303, 15);
            label15.TabIndex = 31;
            label15.Text = "Número de veces que cada hilo entró a la sección crítica";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(188, 331);
            label8.Name = "label8";
            label8.Size = new Size(42, 15);
            label8.TabIndex = 33;
            label8.Text = "Estado";
            // 
            // comboBoxVersion
            // 
            comboBoxVersion.BackColor = Color.Black;
            comboBoxVersion.ForeColor = Color.White;
            comboBoxVersion.FormattingEnabled = true;
            comboBoxVersion.Location = new Point(82, 251);
            comboBoxVersion.Name = "comboBoxVersion";
            comboBoxVersion.Size = new Size(225, 23);
            comboBoxVersion.TabIndex = 34;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1078, 489);
            Controls.Add(comboBoxVersion);
            Controls.Add(label8);
            Controls.Add(labelCriticalSectionCount);
            Controls.Add(label15);
            Controls.Add(labelNonCriticalSectionTime);
            Controls.Add(label13);
            Controls.Add(labelCriticalSectionTime);
            Controls.Add(label11);
            Controls.Add(labelAvgTimePerIteration);
            Controls.Add(label9);
            Controls.Add(labelExecutionTime);
            Controls.Add(label7);
            Controls.Add(listViewThreads);
            Controls.Add(Historial);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(buttonClear);
            Controls.Add(labelProgress);
            Controls.Add(numericUpDownThreads);
            Controls.Add(buttonSaveLog);
            Controls.Add(trackBarSpeed);
            Controls.Add(buttonStart);
            Controls.Add(numericUpDown1);
            Controls.Add(progressBar1);
            Controls.Add(richTextBox1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            ForeColor = SystemColors.ButtonFace;
            Name = "Form1";
            Text = "Algoritmo de Dekker";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownThreads).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private TextBox textBox2;
        private RichTextBox richTextBox1;
        private ProgressBar progressBar1;
        private NumericUpDown numericUpDown1;
        private Button buttonStart;
        private TrackBar trackBarSpeed;
        private Button buttonSaveLog;
        private ListView listViewStats;
        private NumericUpDown numericUpDownThreads;
        private Label labelProgress;
        private Button buttonClear;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label Historial;
        private ListView listViewThreads;
        private Label label7;
        private Label labelExecutionTime;
        private Label labelAvgTimePerIteration;
        private Label label9;
        private Label labelCriticalSectionTime;
        private Label label11;
        private Label labelNonCriticalSectionTime;
        private Label label13;
        private Label labelCriticalSectionCount;
        private Label label15;
        private Label label8;
        private ComboBox comboBoxVersion;
    }
}
