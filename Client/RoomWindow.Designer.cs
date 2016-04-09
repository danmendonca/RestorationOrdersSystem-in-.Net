partial class RoomWindow {
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
            this.btnTableConsult = new System.Windows.Forms.Button();
            this.btnMkReq = new System.Windows.Forms.Button();
            this.comboBoxTable = new System.Windows.Forms.ComboBox();
            this.comboBoxProduct = new System.Windows.Forms.ComboBox();
            this.spinnerQuantity = new System.Windows.Forms.NumericUpDown();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.listViewRequests = new System.Windows.Forms.ListView();
            this.reqNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableNr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.product = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.quantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnReqDelivered = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTableConsult
            // 
            this.btnTableConsult.Location = new System.Drawing.Point(422, 9);
            this.btnTableConsult.Name = "btnTableConsult";
            this.btnTableConsult.Size = new System.Drawing.Size(75, 23);
            this.btnTableConsult.TabIndex = 1;
            this.btnTableConsult.Text = "Consult";
            this.btnTableConsult.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnTableConsult.UseVisualStyleBackColor = true;
            this.btnTableConsult.Click += new System.EventHandler(this.btnTableConsult_Click);
            // 
            // btnMkReq
            // 
            this.btnMkReq.Location = new System.Drawing.Point(422, 39);
            this.btnMkReq.Name = "btnMkReq";
            this.btnMkReq.Size = new System.Drawing.Size(75, 23);
            this.btnMkReq.TabIndex = 2;
            this.btnMkReq.Text = "Request";
            this.btnMkReq.UseVisualStyleBackColor = true;
            this.btnMkReq.Click += new System.EventHandler(this.buttonMkReq_Click);
            // 
            // comboBoxTable
            // 
            this.comboBoxTable.FormattingEnabled = true;
            this.comboBoxTable.Location = new System.Drawing.Point(1, 12);
            this.comboBoxTable.Name = "comboBoxTable";
            this.comboBoxTable.Size = new System.Drawing.Size(80, 21);
            this.comboBoxTable.TabIndex = 3;
            // 
            // comboBoxProduct
            // 
            this.comboBoxProduct.FormattingEnabled = true;
            this.comboBoxProduct.Location = new System.Drawing.Point(87, 12);
            this.comboBoxProduct.Name = "comboBoxProduct";
            this.comboBoxProduct.Size = new System.Drawing.Size(217, 21);
            this.comboBoxProduct.TabIndex = 4;
            // 
            // spinnerQuantity
            // 
            this.spinnerQuantity.Location = new System.Drawing.Point(324, 12);
            this.spinnerQuantity.Name = "spinnerQuantity";
            this.spinnerQuantity.Size = new System.Drawing.Size(48, 20);
            this.spinnerQuantity.TabIndex = 5;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(1, 39);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(371, 20);
            this.textBoxDescription.TabIndex = 6;
            // 
            // listViewRequests
            // 
            this.listViewRequests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.reqNumber,
            this.tableNr,
            this.product,
            this.quantity,
            this.state});
            this.listViewRequests.FullRowSelect = true;
            this.listViewRequests.GridLines = true;
            this.listViewRequests.HideSelection = false;
            this.listViewRequests.Location = new System.Drawing.Point(1, 65);
            this.listViewRequests.MultiSelect = false;
            this.listViewRequests.Name = "listViewRequests";
            this.listViewRequests.Size = new System.Drawing.Size(543, 261);
            this.listViewRequests.TabIndex = 1;
            this.listViewRequests.UseCompatibleStateImageBehavior = false;
            this.listViewRequests.View = System.Windows.Forms.View.Details;
            // 
            // reqNumber
            // 
            this.reqNumber.Text = "Request";
            this.reqNumber.Width = 58;
            // 
            // tableNr
            // 
            this.tableNr.Text = "Table";
            this.tableNr.Width = 48;
            // 
            // product
            // 
            this.product.Text = "Product";
            this.product.Width = 289;
            // 
            // quantity
            // 
            this.quantity.Text = "Quantity";
            this.quantity.Width = 73;
            // 
            // state
            // 
            this.state.Text = "State";
            this.state.Width = 71;
            // 
            // btnReqDelivered
            // 
            this.btnReqDelivered.Location = new System.Drawing.Point(12, 332);
            this.btnReqDelivered.Name = "btnReqDelivered";
            this.btnReqDelivered.Size = new System.Drawing.Size(117, 23);
            this.btnReqDelivered.TabIndex = 8;
            this.btnReqDelivered.Text = "Request Delivered";
            this.btnReqDelivered.UseVisualStyleBackColor = true;
            this.btnReqDelivered.Click += new System.EventHandler(this.btnReqDelivered_Click);
            // 
            // RoomWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 367);
            this.Controls.Add(this.btnReqDelivered);
            this.Controls.Add(this.listViewRequests);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.spinnerQuantity);
            this.Controls.Add(this.comboBoxProduct);
            this.Controls.Add(this.comboBoxTable);
            this.Controls.Add(this.btnMkReq);
            this.Controls.Add(this.btnTableConsult);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RoomWindow";
            this.Text = "Room";
            ((System.ComponentModel.ISupportInitialize)(this.spinnerQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

  }

  #endregion
  private System.Windows.Forms.Button btnTableConsult;
  private System.Windows.Forms.Button btnMkReq;
    private System.Windows.Forms.ComboBox comboBoxTable;
    private System.Windows.Forms.ComboBox comboBoxProduct;
    private System.Windows.Forms.NumericUpDown spinnerQuantity;
    private System.Windows.Forms.TextBox textBoxDescription;
    private System.Windows.Forms.ListView listViewRequests;
    private System.Windows.Forms.ColumnHeader reqNumber;
    private System.Windows.Forms.ColumnHeader tableNr;
    private System.Windows.Forms.ColumnHeader product;
    private System.Windows.Forms.ColumnHeader quantity;
    private System.Windows.Forms.ColumnHeader state;
    private System.Windows.Forms.Button btnReqDelivered;
}

