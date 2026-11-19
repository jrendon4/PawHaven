namespace PawHaven;

public class MainForm : Form
{
    private readonly AnimalManager manager = new AnimalManager();

    private readonly DataGridView grid = new DataGridView();
    private readonly ComboBox cboType = new ComboBox();
    private readonly TextBox txtName = new TextBox();
    private readonly TextBox txtBreed = new TextBox();
    private readonly NumericUpDown numAge = new NumericUpDown();
    private readonly ComboBox cboGender = new ComboBox();
    private readonly TextBox txtHealth = new TextBox();
    private readonly ComboBox cboStatus = new ComboBox();
    private readonly Label lblExtra = new Label();
    private readonly TextBox txtExtra = new TextBox();

    private int selectedId = 0;          // 0 = nothing selected
    private bool loadingGrid = false;    // stops events while the grid refreshes

    public MainForm()
    {
        Text = "PawHaven - Animal Record Management";
        Size = new Size(1000, 560);
        MinimumSize = new Size(900, 500);
        StartPosition = FormStartPosition.CenterScreen;

        BuildGrid();
        BuildForm();
        ClearForm();
    }

    // ---------- Layout ----------
    private void BuildGrid()
    {
        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowHeadersVisible = false;

        grid.Columns.Add("Id", "ID");
        grid.Columns.Add("Type", "Type");
        grid.Columns.Add("Name", "Name");
        grid.Columns.Add("Breed", "Breed");
        grid.Columns.Add("Age", "Age");
        grid.Columns.Add("Gender", "Gender");
        grid.Columns.Add("Health", "Health");
        grid.Columns.Add("Status", "Status");
        grid.Columns.Add("Details", "Details");
        grid.Columns["Id"]!.FillWeight = 30;
        grid.Columns["Age"]!.FillWeight = 30;

        grid.SelectionChanged += Grid_SelectionChanged;

        Controls.Add(grid);   // add the Fill control first
    }

    private void BuildForm()
    {
        TableLayoutPanel right = new TableLayoutPanel();
        right.Dock = DockStyle.Right;
        right.Width = 330;
        right.Padding = new Padding(10);
        right.ColumnCount = 2;
        right.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        cboType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboType.Items.AddRange(new object[] { "Dog", "Cat" });
        cboType.SelectedIndexChanged += (s, e) => UpdateExtraLabel();

        cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGender.Items.AddRange(new object[] { "Male", "Female" });

        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
       cboStatus.Items.AddRange(new object[] { AnimalStatus.Available, AnimalStatus.Adopted, AnimalStatus.UnderCare });

        numAge.Minimum = 0;
        numAge.Maximum = 40;

        int row = 0;
        AddRow(right, ref row, MakeLabel("Type:"), cboType);
        AddRow(right, ref row, MakeLabel("Name:"), txtName);
        AddRow(right, ref row, MakeLabel("Breed:"), txtBreed);
        AddRow(right, ref row, MakeLabel("Age:"), numAge);
        AddRow(right, ref row, MakeLabel("Gender:"), cboGender);
        AddRow(right, ref row, MakeLabel("Health info:"), txtHealth);
        AddRow(right, ref row, MakeLabel("Status:"), cboStatus);

        lblExtra.AutoSize = true;
        lblExtra.Anchor = AnchorStyles.Left;
        lblExtra.Margin = new Padding(3, 8, 3, 3);
        AddRow(right, ref row, lblExtra, txtExtra);

        // Buttons
        Button btnAdd = new Button { Text = "Add", Width = 70 };
        Button btnUpdate = new Button { Text = "Update", Width = 70 };
        Button btnDelete = new Button { Text = "Delete", Width = 70 };
        Button btnClear = new Button { Text = "Clear", Width = 70 };
        btnAdd.Click += BtnAdd_Click;
        btnUpdate.Click += BtnUpdate_Click;
        btnDelete.Click += BtnDelete_Click;
        btnClear.Click += (s, e) => ClearForm();

        FlowLayoutPanel buttons = new FlowLayoutPanel();
        buttons.AutoSize = true;
        buttons.Margin = new Padding(0, 15, 0, 0);
        buttons.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete, btnClear });
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.Controls.Add(buttons, 0, row);
        right.SetColumnSpan(buttons, 2);
        row++;

        right.RowCount = row;
        Controls.Add(right);
    }

    private static Label MakeLabel(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(3, 8, 3, 3)
        };
    }

    private static void AddRow(TableLayoutPanel panel, ref int row, Label label, Control input)
    {
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.Controls.Add(label, 0, row);
        input.Dock = DockStyle.Fill;
        panel.Controls.Add(input, 1, row);
        row++;
    }

    private void UpdateExtraLabel()
    {
        lblExtra.Text = cboType.SelectedIndex == 1 ? "Color:" : "Size:";
    }

    // ---------- Button actions ----------
    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (!InputIsValid()) return;

        Animal animal;
        if (cboType.SelectedIndex == 0)
            animal = new Dog { Size = txtExtra.Text.Trim() };
        else
            animal = new Cat { Color = txtExtra.Text.Trim() };

        ReadFieldsInto(animal);
        manager.AddAnimal(animal);

        RefreshGrid();
        ClearForm();
        MessageBox.Show("Animal added.", "PawHaven");
    }

    private void BtnUpdate_Click(object? sender, EventArgs e)
    {
        if (selectedId == 0)
        {
            MessageBox.Show("Select an animal from the list first.", "PawHaven");
            return;
        }

        Animal? animal = manager.FindById(selectedId);
        if (animal == null) return;
        if (!InputIsValid()) return;

        ReadFieldsInto(animal);
        if (animal is Dog dog) dog.Size = txtExtra.Text.Trim();
        else if (animal is Cat cat) cat.Color = txtExtra.Text.Trim();

        RefreshGrid();
        ClearForm();
        MessageBox.Show("Record updated.", "PawHaven");
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (selectedId == 0)
        {
            MessageBox.Show("Select an animal from the list first.", "PawHaven");
            return;
        }

        DialogResult result = MessageBox.Show("Delete this animal record?", "PawHaven",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result != DialogResult.Yes) return;

        manager.DeleteAnimal(selectedId);
        RefreshGrid();
        ClearForm();
    }

    // ---------- Helpers ----------
    private bool InputIsValid()
    {
        if (cboType.SelectedIndex < 0 || cboGender.SelectedIndex < 0)
        {
            MessageBox.Show("Please choose a type and gender.", "PawHaven");
            return false;
        }
        if (txtName.Text.Trim() == "" || txtBreed.Text.Trim() == "" || txtExtra.Text.Trim() == "")
        {
            MessageBox.Show("Name, breed, and " + lblExtra.Text.TrimEnd(':').ToLower() + " are required.", "PawHaven");
            return false;
        }
        return true;
    }

    private void ReadFieldsInto(Animal animal)
    {
        animal.Name = txtName.Text.Trim();
        animal.Breed = txtBreed.Text.Trim();
        animal.Age = (int)numAge.Value;
        animal.Gender = cboGender.SelectedItem!.ToString()!;
        animal.HealthInfo = txtHealth.Text.Trim();
        animal.Status = (AnimalStatus)cboStatus.SelectedItem!;
    }

    private void RefreshGrid()
    {
        loadingGrid = true;
        grid.Rows.Clear();
        foreach (Animal a in manager.GetAll())
        {
            grid.Rows.Add(a.AnimalId, a.GetAnimalType(), a.Name, a.Breed, a.Age,
                a.Gender, a.HealthInfo, a.Status, a.GetDetails());
        }
        grid.ClearSelection();
        loadingGrid = false;
    }

    private void Grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (loadingGrid || grid.SelectedRows.Count == 0) return;

        int id = Convert.ToInt32(grid.SelectedRows[0].Cells["Id"].Value);
        Animal? animal = manager.FindById(id);
        if (animal == null) return;

        selectedId = id;
        cboType.SelectedIndex = animal is Dog ? 0 : 1;
        cboType.Enabled = false;   // type can't change on an existing record
        txtName.Text = animal.Name;
        txtBreed.Text = animal.Breed;
        numAge.Value = animal.Age > 40 ? 40 : animal.Age;
        cboGender.SelectedItem = animal.Gender;
        txtHealth.Text = animal.HealthInfo;
        cboStatus.SelectedItem = animal.Status;

        if (animal is Dog dog) txtExtra.Text = dog.Size;
        else if (animal is Cat cat) txtExtra.Text = cat.Color;
    }

    private void ClearForm()
    {
        selectedId = 0;
        cboType.Enabled = true;
        cboType.SelectedIndex = 0;
        txtName.Clear();
        txtBreed.Clear();
        numAge.Value = 0;
        cboGender.SelectedIndex = 0;
        txtHealth.Clear();
        cboStatus.SelectedIndex = 0;
        txtExtra.Clear();
        UpdateExtraLabel();
        grid.ClearSelection();
    }
}
