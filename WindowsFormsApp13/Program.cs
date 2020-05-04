using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class
    Form1 : System.Windows.Forms.Form
{
    public Form1()
    {
        this.Load += new EventHandler(Form1_Load);
    }

    #region Form1 Initialize
    /// <summary>
    /// Set conditions for loading form1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form1_Load(System.Object sender, System.EventArgs e)
    {
        string fileName = "C:\\temp\\Sample.txt";
        SetupLayout();
        InitializeDataGridView();

        SetupDataGridView(fileName);

        //Load all default values for control 
        PopulateDataGridView();

        //Autosize header column length to text size
        dataGrid.AutoResizeColumns();

    }

    private void SetupLayout()
    {
        this.Size = new Size(750, 300);

    }

    #endregion

    #region DataGrid Initialize
    /// <summary>
    /// Initialize the datagrid
    /// </summary>
    private void InitializeDataGridView()
    {
        // Initialize basic DataGridView properties.
        dataGrid.Dock = DockStyle.Fill;
        dataGrid.BackgroundColor = Color.LightGray;
        dataGrid.BorderStyle = BorderStyle.Fixed3D;
        dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        // Set property values appropriate for read-only display and 
        // limited interactivity. 
        dataGrid.AllowUserToAddRows = false;
        dataGrid.AllowUserToDeleteRows = false;
        dataGrid.AllowUserToOrderColumns = false;
        //dataGrid.ReadOnly = true;
        dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //dataGrid.MultiSelect = false;
        dataGrid.AllowUserToResizeRows = true;
        dataGrid.AllowUserToResizeColumns = false;
        //dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        //songsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        //songsDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

        //Fill the window with the dataGridView 
        dataGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left);
        //Allows the DataGridView to resize with window
        //dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        CreateNestedMenu();

    }

    /// <summary>
    /// Add a horizontal scrollbar
    /// </summary>
    private void InitializeMyScrollBar()
    {
        // Create and initialize a VScrollBar.
        VScrollBar vScrollBar1 = new VScrollBar();

        // Dock the scroll bar to the right side of the form.
        vScrollBar1.Dock = DockStyle.Right;

        // Add the scroll bar to the form.
        Controls.Add(vScrollBar1);
    }

    #endregion

    #region Nested Menu 
    /// <summary>
    /// Creates a nested menu for each click 
    /// </summary>
    private void CreateNestedMenu()
    {
        dropDownMenu.Items.Add("Cut").Name = "Cut";
        dropDownMenu.Items.Add("Delete", Image.FromFile("c:\\temp\\Delete.png"), deleteItem_Click);
        dropDownMenu.Items.Add("Copy").Name = "Copy";
        dropDownMenu.Items.Add("Paste").Name = "Paste";

        //Separator
        dropDownMenu.Items.Add(new ToolStripSeparator());

        dropDownMenu.Items.Add("Remove Clients From Group").Name = "4";
        dropDownMenu.Items.Add("Delete Clients From Smart Control").Name = "5";

        //Separator
        dropDownMenu.Items.Add(new ToolStripSeparator());

        //Submenu 1
        ToolStripMenuItem firstSubitem = new ToolStripMenuItem("Protection Mode");
        dropDownMenu.Items.Add(firstSubitem);

        ToolStripMenuItem subSubItem2 = new ToolStripMenuItem("Enable Selected Clients", Image.FromFile("c:\\temp\\Delete.png"), ItemB_Click);
        firstSubitem.DropDownItems.Add(subSubItem2);

        ToolStripMenuItem subSubItem = new ToolStripMenuItem("Disable Selected Clients", Image.FromFile("c:\\temp\\Delete.png"), deleteItem_Click, (Keys)Shortcut.F1);
        firstSubitem.DropDownItems.Add(subSubItem);

        firstSubitem.DropDownItems.Add(new ToolStripSeparator());

        ToolStripMenuItem subSubItem3 = new ToolStripMenuItem("Block USB");
        firstSubitem.DropDownItems.Add(subSubItem3);

        ToolStripMenuItem subSubItem4 = new ToolStripMenuItem("Unblock USB");
        firstSubitem.DropDownItems.Add(subSubItem4);

        //List<string> items = new List<string>() { "item1", "item2", "item3" };

        //Submenu 2
        submenu = new ToolStripMenuItem();
        submenu.Text = "Misc.Configuration";
        dropDownMenu.Items.Add(submenu);

        //Subitem 2a
        item = new ToolStripMenuItem();
        item.Text = "1";
        submenu.DropDownItems.Add(item);

        //Subitem 2b
        item = new ToolStripMenuItem();
        item.Text = "2";
        submenu.DropDownItems.Add(item);


        // Bind the MainMenu to Form1.
        //this.Controls.Add(dropDownMenu);
    }
    #endregion

    #region Callback Function 
    /// <summary>
    /// Create an event when a submenu item is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    private void deleteItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Delete Event");
    }

    private void ItemB_Click(object sender, EventArgs e)
    {
        MessageBox.Show("A");
    }

    #endregion

    #region Setup Datagrid and Read Text
    /// <summary>
    /// Set up the data grid and read words from a text file into column and rows
    /// </summary>
    /// <param name="fileName"></param>
    private void loadColumnTitle(string fileName)
    {
        String line;
        try
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(fileName);

            //Read the first line of text
            line = sr.ReadLine();

            //Read the first line which should be the column titles
            if (line != null)
            {
                //Split the string into words
                columnTitles = line.Split(',');
                Debug.WriteLine(line);

            }

            //Read the body, and populate the dataGridData array
            line = sr.ReadLine();
            int i = 0;

            dataGridData = new string[1];
            while (line != null)
            {
                dataGridData[i] = line;
                i += 1;
                line = sr.ReadLine();
                Array.Resize(ref dataGridData, dataGridData.Length + 1);
            }
            //close the file
            sr.Close();
        }
        catch (Exception e)
        {
            Debug.WriteLine("Exception: " + e.Message);
        }
    }

    private void SetupDataGridView(string fileName)
    {
        //load in columns from data file FileName
        loadColumnTitle(fileName);
        this.Controls.Add(dataGrid);

        dataGrid.ColumnCount = columnTitles.Length;
        dataGrid.ColumnHeadersVisible = true;

        dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
        dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        //Creates bold for the column font
        //        dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(dataGrid.Font, FontStyle.Bold);

        dataGrid.Name = "dataGrid";
        dataGrid.Location = new Point(8, 8);
        dataGrid.Size = new Size(1000, 250);
        dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
        dataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        dataGrid.GridColor = Color.Black;
        dataGrid.RowHeadersVisible = false;
        dataGrid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        for (int i = 0; i < columnTitles.Length; i++)
        {
            dataGrid.Columns[i].Name = columnTitles[i];
        }

        dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGrid.MultiSelect = false;
        dataGrid.Dock = DockStyle.Fill;

        //Set MouseClick Event
        dataGrid.MouseClick += new MouseEventHandler(this.songsDataGridView_Click);
    }

    #endregion

    #region Insert text to datagrid using an array through text reader
    /// <summary>
    /// Add text to the datagrid i.e. Desktop number, PC number, software, software version etc. 
    /// </summary>
    private void PopulateDataGridView()
    {
        string[] words;

        for (int i = 0; i < (dataGridData.Length) - 1; i++)
        {
            words = dataGridData[i].Split(',');
            dataGrid.Rows.Add(words);

        }

        Debug.WriteLine((dataGridData.Length));

        //
        DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
        cmb.HeaderText = "Select Data";
        cmb.Name = "cmb";
        cmb.MaxDropDownItems = 4;
        cmb.Items.Add("True");
        cmb.Items.Add("Unknown");
        cmb.Items.Add("False");
        dataGrid.Columns.Add(cmb);

        DataGridViewComboBoxColumn test = new DataGridViewComboBoxColumn();
        test.Name = "test";
        test.Items.Add("True");
        test.Items.Add("False");
        //i.DataGridView();

        dataGrid.Columns.Add(test);
        //
        
        for (int i = 0; i < (columnTitles.Length); i++)
        {
            dataGrid.Columns[i].DisplayIndex = i;
            Debug.Write(i);
         }

        Debug.WriteLine((columnTitles.Length));                      
    }

    #endregion

    #region DropDownmenu Filter 
    
    #endregion

    #region Dropdown menu conditions 
    /// <summary>
    /// shows the dropdown menu only if the user right clicks on one of the rows 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void songsDataGridView_Click(object sender, MouseEventArgs e)
    {
        int position_xy_mouse_row = dataGrid.HitTest(e.X, e.Y).RowIndex;
        if ((e.Button == MouseButtons.Right) && (position_xy_mouse_row >= 0))
        {
            for (int index = 0; index < this.dataGrid.Rows.Count; index++)
            {
                if (this.dataGrid.Rows[index].Selected == true)
                {
                    //MessageBox.Show($"Row {index} is selected.");
                    CreateMyMainMenu(e);
                    return;
                }
            }

        }
    }
    private void CreateMyMainMenu(MouseEventArgs e)
    {
        dropDownMenu.Show(dataGrid, new Point(e.X, e.Y));
    }
    #endregion

    #region Static Main Method
    [STAThreadAttribute()]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }

    #endregion

    #region Declare Variables
    /// <summary>
    /// Declare private variables and arrays
    /// </summary>

    //Datagrid
    private DataGridView dataGrid = new DataGridView();

    //Nested menu (context menu)
    private ContextMenuStrip dropDownMenu = new ContextMenuStrip();

    //Submenus 
    private ToolStripMenuItem item, submenu;

    //ColumnTitles
    private string[] columnTitles;

    //DataGrid Data
    private string[] dataGridData;

    //Combo box, dropdown menu 
    
    //unused
    private void button1_Click(object sender, EventArgs e)
    {
        DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
        cmb.HeaderText = "Select Data";
        cmb.Name = "cmb";
        cmb.MaxDropDownItems = 2;
        cmb.Items.Add("True");
        cmb.Items.Add("False");
        cmb.Items.Add("Unknown");
        dataGrid.Columns.Add(cmb);

        DataGridViewComboBoxCell i = new DataGridViewComboBoxCell();
        i.Value = "test";
        i.Items.Add("True");
        i.Items.Add("False");
 
        dataGrid.Rows.Add(i);
     }

    private void Form1_Load_1(object sender, EventArgs e)
    {

    }

    #endregion

    /// <summary>
    /// Initialize the form 
    /// </summary>
    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);

    }

}