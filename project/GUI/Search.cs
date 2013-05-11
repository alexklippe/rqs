﻿/*   
 *  RQS
 *  Requirement Searching Utility
 *  Copyright (C) Fuks Alexander 2013
 *  
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 *  
 *  Fuks Alexander, hereby disclaims all copyright
 *  interest in the program "RQS"
 *  (which makes passes at compilers)
 *  written by Alexander Fuks.
 * 
 *  Alexander Fuks, 10 May 2013.
 */

using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace RQS.GUI
{
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();
        }

        private FRSearch FRSearch = new FRSearch();
        private SmartDataGridView DataGridView = new SmartDataGridView("SearchResults", 6);
        private int[] LastMouseDownLocation = new int[] { 0, 0 };

        private void Search_Load(object sender, System.EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            DataGridView.Dock = DockStyle.Fill;

            DataGridView.Columns.Add("", "#");
            DataGridView.Columns.Add("", "Source");
            DataGridView.Columns.Add("", "FR ID");
            DataGridView.Columns.Add("", "FR TMS Task");
            DataGridView.Columns.Add("", "FR Text");
            DataGridView.Columns.Add("", "CCP");

            DataGridView.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
            DataGridView.MouseDown += new MouseEventHandler(DataGridView_MouseDown);

            DataGridView.MultiSelect = true;
            DataGridView.ContextMenuStrip = contextMenuStrip1;

            tableLayoutPanel1.Controls.Add(DataGridView, 0, 1);
        }

        void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                LastMouseDownLocation[0] = DataGridView.HitTest(e.X, e.Y).RowIndex;
                LastMouseDownLocation[1] = DataGridView.HitTest(e.X, e.Y).ColumnIndex;

                if (LastMouseDownLocation[0] >= 0)
                {
                    DataGridView.ClearSelection();
                    DataGridView.Rows[LastMouseDownLocation[0]].Selected = true;
                }
            }
        }

        private void bSearch_Click(object sender, System.EventArgs e)
        {
            DataGridView.Rows.Clear();

            List<FR> FRs = FRSearch.Search((FRSearch.SearchBy)comboBox1.SelectedIndex, 
                textBox1.Text, checkBox1.Checked);
            for (int a=0; a < FRs.Count; a++)
            {
                DataGridView.Rows.Add(
                    (a + 1).ToString(),
                    FRs[a].FoundInFile,
                    FRs[a].FRID,
                    FRs[a].FRTMSTask,
                    FRs[a].FRText,
                    FRs[a].CCP);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    bSearch_Click(this, null);
                    break;
                case Keys.Up:
                    if (comboBox1.SelectedIndex > 0)
                    {
                        comboBox1.SelectedIndex--;
                    }
                    break;
                case Keys.Down:
                    if (comboBox1.SelectedIndex < comboBox1.Items.Count - 1)
                    {
                        comboBox1.SelectedIndex++;
                    }
                    break;
            }
        }

        private void contextCopyCell_Click(object sender, System.EventArgs e)
        {
            if (LastMouseDownLocation[0] >= 0 &&
                LastMouseDownLocation[1] >= 0 &&
                DataGridView.Rows.Count >= LastMouseDownLocation[0] &&
                DataGridView.Columns.Count >= LastMouseDownLocation[1])
            {
                Clipboard.SetText(DataGridView.Rows[LastMouseDownLocation[0]].
                    Cells[LastMouseDownLocation[1]].Value.ToString());
            }
        }

        void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Clipboard.SetText(DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            }
        }

        private void contextCopyRows_Click(object sender, System.EventArgs e)
        {
            string text = "";
            foreach (DataGridViewRow row in DataGridView.SelectedRows)
            {
                for (int a = 0; a < DataGridView.Columns.Count; a++)
                {
                    if (!DataGridView.Columns[a].Visible)
                    {
                        continue;
                    }
                    for (int b = 0; b < DataGridView.Columns.Count; b++)
                    {
                        if (DataGridView.Columns[b].DisplayIndex == a)
                        {
                            text += row.Cells[b].Value.ToString() + ";";
                        }
                    }
                }
                text += Environment.NewLine;
            }
            Clipboard.SetText(text);
        }

        private void contextCustomize_Click(object sender, EventArgs e)
        {
            DataGridView.ShowSetup(this.ParentForm);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextCopyCell.Enabled = DataGridView.SelectedRows.Count > 0;
            contextCopyRows.Enabled = DataGridView.SelectedRows.Count > 0;
        }
    }
}
