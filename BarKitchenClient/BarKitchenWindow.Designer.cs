namespace BarKitchenClient
{
    partial class BarKitchenWindow
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.updateStateBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.request = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.table = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.product = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.quantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.service = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.stComboBox = new System.Windows.Forms.ComboBox();
            this.startAppBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 828F));
            this.tableLayoutPanel2.Controls.Add(this.updateStateBtn);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 516);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(828, 45);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // updateStateBtn
            // 
            this.updateStateBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.updateStateBtn.AutoSize = true;
            this.updateStateBtn.Location = new System.Drawing.Point(3, 8);
            this.updateStateBtn.Name = "updateStateBtn";
            this.updateStateBtn.Size = new System.Drawing.Size(144, 30);
            this.updateStateBtn.TabIndex = 0;
            this.updateStateBtn.Text = "Update State";
            this.updateStateBtn.UseVisualStyleBackColor = true;
            this.updateStateBtn.Click += new System.EventHandler(this.UpdateRequestState_Click);
            this.updateStateBtn.Enabled = false;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.request,
            this.table,
            this.product,
            this.quantity,
            this.state,
            this.service});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(3, 54);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(828, 456);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // request
            // 
            this.request.Text = "Pedido";
            this.request.Width = 80;
            // 
            // table
            // 
            this.table.Text = "Mesa";
            this.table.Width = 80;
            // 
            // product
            // 
            this.product.Text = "Produto";
            this.product.Width = 80;
            // 
            // quantity
            // 
            this.quantity.Text = "Quantidade";
            this.quantity.Width = 80;
            // 
            // state
            // 
            this.state.Text = "Estado";
            this.state.Width = 80;
            // 
            // service
            // 
            this.service.Text = "Serviço";
            this.service.Width = 80;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.941521F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.05848F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(834, 565);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.Controls.Add(this.stComboBox);
            this.flowLayoutPanel1.Controls.Add(this.startAppBtn);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(828, 45);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.stComboBox.FormattingEnabled = true;
            this.stComboBox.Items.AddRange(new object[] {
            PreparationRoomID.Bar,
            PreparationRoomID.Kitchen});
            this.stComboBox.Location = new System.Drawing.Point(8, 8);
            this.stComboBox.Name = "stComboBox";
            this.stComboBox.Size = new System.Drawing.Size(121, 28);
            this.stComboBox.TabIndex = 0;
            this.stComboBox.SelectedIndex = 0;
            // 
            // startAppBtn
            // 
            this.startAppBtn.AutoSize = true;
            this.startAppBtn.Location = new System.Drawing.Point(135, 8);
            this.startAppBtn.Name = "startAppBtn";
            this.startAppBtn.Size = new System.Drawing.Size(87, 30);
            this.startAppBtn.TabIndex = 1;
            this.startAppBtn.Text = "Start App";
            this.startAppBtn.UseVisualStyleBackColor = true;
            this.startAppBtn.Click += new System.EventHandler(this.startAppBtn_Click);
            // 
            // BarKitchenWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 565);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BarKitchenWindow";
            this.Text = "Bar Kitchen App";
            this.Load += new System.EventHandler(this.BarKitchenWindow_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button updateStateBtn;
        private System.Windows.Forms.Button startAppBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader request;
        private System.Windows.Forms.ColumnHeader table;
        private System.Windows.Forms.ColumnHeader product;
        private System.Windows.Forms.ColumnHeader quantity;
        private System.Windows.Forms.ColumnHeader state;
        private System.Windows.Forms.ColumnHeader service;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox stComboBox;
        
    }
}

