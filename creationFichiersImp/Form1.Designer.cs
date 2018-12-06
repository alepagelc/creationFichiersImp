namespace creationFichiersImp
{
    partial class FormCreateImp
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxFicCSV = new System.Windows.Forms.TextBox();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.buttonChoixFichier = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxServeur = new System.Windows.Forms.TextBox();
            this.comboBoxBase = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataSetLstBases = new System.Data.DataSet();
            this.openFileDialogCSV = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetLstBases)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxFicCSV
            // 
            this.textBoxFicCSV.Location = new System.Drawing.Point(6, 19);
            this.textBoxFicCSV.Name = "textBoxFicCSV";
            this.textBoxFicCSV.Size = new System.Drawing.Size(342, 20);
            this.textBoxFicCSV.TabIndex = 0;
            // 
            // buttonConvert
            // 
            this.buttonConvert.Location = new System.Drawing.Point(164, 180);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(75, 22);
            this.buttonConvert.TabIndex = 2;
            this.buttonConvert.Text = "Convertir";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // buttonChoixFichier
            // 
            this.buttonChoixFichier.Location = new System.Drawing.Point(355, 15);
            this.buttonChoixFichier.Name = "buttonChoixFichier";
            this.buttonChoixFichier.Size = new System.Drawing.Size(26, 26);
            this.buttonChoixFichier.TabIndex = 4;
            this.buttonChoixFichier.Text = "...";
            this.buttonChoixFichier.UseVisualStyleBackColor = true;
            this.buttonChoixFichier.Click += new System.EventHandler(this.buttonChoixFichier_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(44, 36);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(35, 20);
            this.textBoxPort.TabIndex = 5;
            this.textBoxPort.Text = "5432";
            this.textBoxPort.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxPort_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Adresse / Nom du serveur :";
            // 
            // textBoxServeur
            // 
            this.textBoxServeur.Location = new System.Drawing.Point(241, 36);
            this.textBoxServeur.Name = "textBoxServeur";
            this.textBoxServeur.Size = new System.Drawing.Size(140, 20);
            this.textBoxServeur.TabIndex = 8;
            this.textBoxServeur.Text = "127.0.0.1";
            this.textBoxServeur.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxServeur_KeyUp);
            // 
            // comboBoxBase
            // 
            this.comboBoxBase.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBase.FormattingEnabled = true;
            this.comboBoxBase.Location = new System.Drawing.Point(9, 62);
            this.comboBoxBase.Name = "comboBoxBase";
            this.comboBoxBase.Size = new System.Drawing.Size(372, 21);
            this.comboBoxBase.TabIndex = 9;
            this.comboBoxBase.SelectionChangeCommitted += new System.EventHandler(this.comboBoxBase_SelectionChangeCommitted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonChoixFichier);
            this.groupBox1.Controls.Add(this.textBoxFicCSV);
            this.groupBox1.Location = new System.Drawing.Point(7, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 48);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sélection du fichier à convertir :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxBase);
            this.groupBox2.Controls.Add(this.textBoxServeur);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxPort);
            this.groupBox2.Location = new System.Drawing.Point(7, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(387, 98);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sélectionner le serveur et la base de données pour laquelle déposer les fiches cl" +
    "ients :";
            // 
            // dataSetLstBases
            // 
            this.dataSetLstBases.DataSetName = "NewDataSet";
            // 
            // openFileDialogCSV
            // 
            this.openFileDialogCSV.Filter = "Tous les fichiers|*.*|Fichiers CSV|*.csv";
            this.openFileDialogCSV.FilterIndex = 2;
            // 
            // FormCreateImp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 208);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.groupBox2);
            this.Name = "FormCreateImp";
            this.Text = "Conversion en fichier *.imp";
            this.Load += new System.EventHandler(this.FormCreateImp_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetLstBases)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFicCSV;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.Button buttonChoixFichier;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxServeur;
        private System.Windows.Forms.ComboBox comboBoxBase;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Data.DataSet dataSetLstBases;
        private System.Windows.Forms.OpenFileDialog openFileDialogCSV;
    }
}

