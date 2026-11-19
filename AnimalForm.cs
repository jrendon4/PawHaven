using System.Drawing;
using System.Windows.Forms;
using PawHaven.Models;

namespace PawHaven.UI;

public class AnimalForm : Form
{
    private readonly Animal? _existing;

    private readonly ComboBox cboType = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox txtName = new();
    private readonly TextBox txtBreed = new();
    private readonly NumericUpDown numAge = new() { Minimum = 0, Maximum = 40 };
    private readonly ComboBox cboGender = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox cboStatus = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label lblExtra = new() { Text = "Size", AutoSize = true, Anchor = AnchorStyles.Left };
    private readonly TextBox txtExtra = new();
    private readonly TextBox txtHealth = new() { Multiline = true, Height = 70 };

    public Animal? Result { get; private set; }

    public AnimalForm(Animal? existing = null)
    {
        _existing = existing;

        Text = existing == null ? "Add Animal" : "Edit Animal";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(380, 400);

        cboType.Items.AddRange(new object[] { "Dog", "Cat" });
        cboGender.Items.AddRange(new object[] { "Male", "Female", "Unknown" });
        cboStatus.DataSource = Enum.GetValues<AnimalStatus>();

        cboType.SelectedIndexChanged += (_, _) =>
            lblExtra.Text = cboType.Text == "Dog" ? "Size" : "Color";

        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(12)
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68));

        AddRow(table, "Type", cboType);
        AddRow(table, "Name", txtName);
        AddRow(table, "Breed", txtBreed);
        AddRow(table, "Age", numAge);
        AddRow(table, "Gender", cboGender);
        AddRow(table, "Status", cboStatus);
        AddRow(table, lblExtra, txtExtra);
        AddRow(table, "Health notes", txtHealth);

        var btnOk = new Button { Text = "Save", Width = 90 };
        var btnCancel = new Button { Text = "Cancel", Width = 90, DialogResult = DialogResult.Cancel };
        btnOk.Click += OnSave;

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 48,
            Padding = new Padding(8)
        };
        buttons.Controls.Add(btnCancel);
        buttons.Controls.Add(btnOk);

        Controls.Add(table);
        Controls.Add(buttons);
        CancelButton = btnCancel;

        LoadExisting();
    }

    private static void AddRow(TableLayoutPanel t, string label, Control input) =>
        AddRow(t, new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, input);

    private static void AddRow(TableLayoutPanel t, Label label, Control input)
    {
        input.Dock = DockStyle.Fill;
        input.Margin = new Padding(3, 6, 3, 6);
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.Controls.Add(label);
        t.Controls.Add(input);
    }

    private void LoadExisting()
    {
        if (_existing == null)
        {
            cboType.SelectedIndex = 0;
            cboGender.SelectedIndex = 2;
            return;
        }

        cboType.Text = _existing.GetAnimalType();
        cboType.Enabled = false; // keep the type fixed while editing
        txtName.Text = _existing.Name;
        txtBreed.Text = _existing.Breed;
        numAge.Value = _existing.Age;
        cboGender.Text = _existing.Gender;
        cboStatus.SelectedItem = _existing.Status;
        txtHealth.Text = _existing.HealthNotes;
        txtExtra.Text = _existing switch
        {
            Dog d => d.Size,
            Cat c => c.Color,
            _ => ""
        };
    }

    private void OnSave(object? sender, EventArgs e)
    {
        try
        {
            string extra = txtExtra.Text.Trim();
            Animal animal = cboType.Text == "Dog"
                ? new Dog { Size = extra }
                : new Cat { Color = extra };

            animal.AnimalId = _existing?.AnimalId ?? "";
            animal.Name = txtName.Text;              // throws if empty
            animal.Breed = txtBreed.Text;
            animal.Age = (int)numAge.Value;
            animal.Gender = cboGender.Text;
            animal.Status = (AnimalStatus)cboStatus.SelectedItem!;
            animal.HealthNotes = txtHealth.Text.Trim();
            animal.DateAdmitted = _existing?.DateAdmitted ?? DateTime.Now;

            Result = animal;
            DialogResult = DialogResult.OK;          // closes the dialog
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Invalid input",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
