namespace RegisterGUI
{
    partial class RegisterGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxTable = new System.Windows.Forms.ComboBox();
            this.btnSetTablePaid = new System.Windows.Forms.Button();
            this.listViewTableReqs = new System.Windows.Forms.ListView();
            this.columnProduct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnQuantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnUnitPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // comboBoxTable
            // 
            this.comboBoxTable.FormattingEnabled = true;
            this.comboBoxTable.Location = new System.Drawing.Point(197, 423);
            this.comboBoxTable.Name = "comboBoxTable";
            this.comboBoxTable.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTable.TabIndex = 0;
            this.comboBoxTable.SelectedIndexChanged += new System.EventHandler(this.comboBoxTable_SelectedIndexChanged);
            // 
            // btnSetTablePaid
            // 
            this.btnSetTablePaid.Location = new System.Drawing.Point(324, 423);
            this.btnSetTablePaid.Name = "btnSetTablePaid";
            this.btnSetTablePaid.Size = new System.Drawing.Size(75, 23);
            this.btnSetTablePaid.TabIndex = 1;
            this.btnSetTablePaid.Text = "Pay Table";
            this.btnSetTablePaid.UseVisualStyleBackColor = true;
            this.btnSetTablePaid.Click += new System.EventHandler(this.btnSetTablePaid_Click);
            // 
            // listViewTableReqs
            // 
            this.listViewTableReqs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnProduct,
            this.columnQuantity,
            this.columnUnitPrice,
            this.columnPrice});
            this.listViewTableReqs.FullRowSelect = true;
            this.listViewTableReqs.GridLines = true;
            this.listViewTableReqs.HideSelection = false;
            this.listViewTableReqs.Location = new System.Drawing.Point(12, 12);
            this.listViewTableReqs.MultiSelect = false;
            this.listViewTableReqs.Name = "listViewTableReqs";
            this.listViewTableReqs.Size = new System.Drawing.Size(602, 405);
            this.listViewTableReqs.TabIndex = 1;
            this.listViewTableReqs.UseCompatibleStateImageBehavior = false;
            this.listViewTableReqs.View = System.Windows.Forms.View.Details;
            // 
            // columnProduct
            // 
            this.columnProduct.Text = "Product";
            this.columnProduct.Width = 362;
            // 
            // columnQuantity
            // 
            this.columnQuantity.Text = "Quantity";
            this.columnQuantity.Width = 71;
            // 
            // columnUnitPrice
            // 
            this.columnUnitPrice.Text = "Unit Price";
            this.columnUnitPrice.Width = 86;
            // 
            // columnPrice
            // 
            this.columnPrice.Text = "Price";
            this.columnPrice.Width = 73;
            // 
            // RegisterGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 453);
            this.Controls.Add(this.listViewTableReqs);
            this.Controls.Add(this.btnSetTablePaid);
            this.Controls.Add(this.comboBoxTable);
            this.Name = "RegisterGUI";
            this.Text = "Register";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTable;
        private System.Windows.Forms.Button btnSetTablePaid;
        private System.Windows.Forms.ListView listViewTableReqs;
        private System.Windows.Forms.ColumnHeader columnProduct;
        private System.Windows.Forms.ColumnHeader columnQuantity;
        private System.Windows.Forms.ColumnHeader columnUnitPrice;
        private System.Windows.Forms.ColumnHeader columnPrice;
    }
}

