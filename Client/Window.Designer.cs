partial class Window {
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing) {
    if (disposing && (components != null)) {
      components.Dispose();
    }
    base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent() {
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnMkReq = new System.Windows.Forms.Button();
            this.textBoxReadyReq = new System.Windows.Forms.TextBox();
            this.comboBoxTable = new System.Windows.Forms.ComboBox();
            this.comboBoxProduct = new System.Windows.Forms.ComboBox();
            this.spinnerQuantity = new System.Windows.Forms.NumericUpDown();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(197, 1);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Ligar";
            this.btnConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // btnMkReq
            // 
            this.btnMkReq.Location = new System.Drawing.Point(197, 30);
            this.btnMkReq.Name = "btnMkReq";
            this.btnMkReq.Size = new System.Drawing.Size(75, 23);
            this.btnMkReq.TabIndex = 2;
            this.btnMkReq.Text = "Pedir";
            this.btnMkReq.UseVisualStyleBackColor = true;
            this.btnMkReq.Click += new System.EventHandler(this.buttonMkReq_Click);
            // 
            // textBoxReadyReq
            // 
            this.textBoxReadyReq.Location = new System.Drawing.Point(13, 105);
            this.textBoxReadyReq.Multiline = true;
            this.textBoxReadyReq.Name = "textBoxReadyReq";
            this.textBoxReadyReq.ReadOnly = true;
            this.textBoxReadyReq.Size = new System.Drawing.Size(259, 144);
            this.textBoxReadyReq.TabIndex = 0;
            this.textBoxReadyReq.TabStop = false;
            // 
            // comboBoxTable
            // 
            this.comboBoxTable.FormattingEnabled = true;
            this.comboBoxTable.Location = new System.Drawing.Point(1, 1);
            this.comboBoxTable.Name = "comboBoxTable";
            this.comboBoxTable.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTable.TabIndex = 3;
            // 
            // comboBoxProduct
            // 
            this.comboBoxProduct.FormattingEnabled = true;
            this.comboBoxProduct.Location = new System.Drawing.Point(1, 30);
            this.comboBoxProduct.Name = "comboBoxProduct";
            this.comboBoxProduct.Size = new System.Drawing.Size(121, 21);
            this.comboBoxProduct.TabIndex = 4;
            // 
            // spinnerQuantity
            // 
            this.spinnerQuantity.Location = new System.Drawing.Point(128, 30);
            this.spinnerQuantity.Name = "spinnerQuantity";
            this.spinnerQuantity.Size = new System.Drawing.Size(48, 20);
            this.spinnerQuantity.TabIndex = 5;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(1, 57);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(271, 20);
            this.textBoxDescription.TabIndex = 6;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.spinnerQuantity);
            this.Controls.Add(this.comboBoxProduct);
            this.Controls.Add(this.comboBoxTable);
            this.Controls.Add(this.textBoxReadyReq);
            this.Controls.Add(this.btnMkReq);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Window";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.Window_Load);
            ((System.ComponentModel.ISupportInitialize)(this.spinnerQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

  }

  #endregion
  private System.Windows.Forms.Button btnConnect;
  private System.Windows.Forms.Button btnMkReq;
  private System.Windows.Forms.TextBox textBoxReadyReq;
    private System.Windows.Forms.ComboBox comboBoxTable;
    private System.Windows.Forms.ComboBox comboBoxProduct;
    private System.Windows.Forms.NumericUpDown spinnerQuantity;
    private System.Windows.Forms.TextBox textBoxDescription;
}

