using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ExcelDataReader;
using Word = Microsoft.Office.Interop.Word;

namespace TRPAnalisis
{
    public partial class Task : Form
    {
        private string fileName = string.Empty;
        private int _countQuantityAllGold, _countQuantityAllSilver, _countQuantityAllBronze, _countQuantityAllNotPassed,
        _countQuantityGirlGold, _countQuantityGirlSilver, _countQuantityGirlBronze, _countQuantityGirlNotPassed,
        _countQuantityManGold, _countQuantityManSilver, _countQuantityManBronze, _countQuantityManNotPassed;


        private DataTableCollection tableCollection = null;


        public Task()
        {
            InitializeComponent();
        }

        private void TSMIOpen_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = oFD.ShowDialog();
                if (res == DialogResult.OK)
                {
                    fileName = oFD.FileName;
                    Text = fileName;

                    OpenExcelFile(fileName);
                    Task_Load(sender, e);
                }
                else
                {
                    throw new Exception("Файл не выбран!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void OpenExcelFile(string patch)
        {
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            FileStream stream = File.Open(patch, FileMode.Open, FileAccess.Read);

            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

            DataSet db = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (x) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            tableCollection = db.Tables;
            tscbList.Items.Clear();
            foreach (DataTable tabe in tableCollection)
            {
                tscbList.Items.Add(tabe.TableName);
            }
            tscbList.SelectedIndex = 0;

        }


        private void tscbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable table = tableCollection[Convert.ToString(tscbList.SelectedItem)];
            dgvExcel.DataSource = table;
        }



        private void btnResultCharts_Click(object sender, EventArgs e)
        {

            //подсчёт количества определённых мест
            //int countQuantityAllGold = 0;
            //int countQuantityAllSilver = 0;
            //int countQuantityAllBronze = 0;
            //int countQuantityAllNotPassed = 0;

            _countQuantityAllGold = 0;
            _countQuantityAllSilver = 0;
            _countQuantityAllBronze = 0;
            _countQuantityAllNotPassed = 0;

            for (int i = 0; i < dgvExcel.RowCount - 1; i++)
            {

                if (dgvExcel.Rows[i].Cells["Занятое место в группе (Ранговая)"].Value.ToString() == "1")
                {
                    _countQuantityAllGold++;
                    lAllGold.Text = "Кол-во студентов, сдавших на золотой значок: " + _countQuantityAllGold;

                }
                if (dgvExcel.Rows[i].Cells["Занятое место в группе (Ранговая)"].Value.ToString() == "2")
                {
                    _countQuantityAllSilver++;
                    lAllSilver.Text = "Кол-во студентов, сдавших на серебряный значок: " + _countQuantityAllSilver;

                }
                if (dgvExcel.Rows[i].Cells["Занятое место в группе (Ранговая)"].Value.ToString() == "3")
                {
                    _countQuantityAllBronze++;
                    lAllBronze.Text = "Кол-во студентов, сдавших на бронзовый  значок: " + _countQuantityAllBronze;

                }
                if (dgvExcel.Rows[i].Cells["Занятое место в группе (Ранговая)"].Value.ToString() == "0")
                {
                    _countQuantityAllNotPassed++;
                    lAllNotPassed.Text = "Кол-во студентов, не сдавших норму ГТО: " + _countQuantityAllNotPassed;

                }
            }
            //подсчёт количества определённых мест


            chartAll.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;// тип диаграммы


            chartAll.Titles.Add("Для всей группы по данному виду спорта"); //название диаграммы
            Color[] PieColors = { Color.SkyBlue, Color.LimeGreen, Color.MediumOrchid, Color.LightCoral }; //массив цветов


            if (_countQuantityAllGold != 0)
            {
                chartAll.Series[0].Points.AddY(_countQuantityAllGold);//значение
                chartAll.Series[0].Points[0].Color = PieColors[0]; //цвет диаграммы
                chartAll.Series[0].Points[0].LegendText = "Кол-во студентов, сдавших на золотой значок";//название подписи данных
            }
            if (_countQuantityAllSilver != 0)
            {
                chartAll.Series[0].Points.AddY(_countQuantityAllSilver);//значение
                chartAll.Series[0].Points[1].Color = PieColors[1]; //цвет диаграммы
                chartAll.Series[0].Points[1].LegendText = "Кол-во студентов, сдавших на серебряный значок";//название подписи данных
            }
            if (_countQuantityAllBronze != 0)
            {
                chartAll.Series[0].Points.AddY(_countQuantityAllBronze);//значение
                chartAll.Series[0].Points[2].Color = PieColors[2]; //цвет диаграммы
                chartAll.Series[0].Points[2].LegendText = "Кол-во студентов, сдавших на бронзовый  значок";//название подписи данных
            }
            if (_countQuantityAllNotPassed != 0)
            {
                chartAll.Series[0].Points.AddY(_countQuantityAllNotPassed);//значение
                chartAll.Series[0].Points[3].Color = PieColors[3]; //цвет диаграммы
                chartAll.Series[0].Points[3].LegendText = "Кол-во студентов, не сдавших норму ГТО";//название подписи данных
            }



            //подсчёт количества определённых мест
            _countQuantityGirlGold = 0;
            _countQuantityGirlSilver = 0;
            _countQuantityGirlBronze = 0;
            _countQuantityGirlNotPassed = 0;
            for (int i = 0; i < dgvExcel.RowCount - 1; i++)
            {

                if (dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Value.ToString() == "1")
                {
                    _countQuantityGirlGold++;
                    lGirlGold.Text = "Кол-во девушек, сдавших на золотой значок: " + _countQuantityGirlGold;

                }
                if (dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Value.ToString() == "2")
                {
                    _countQuantityGirlSilver++;
                    lGirlSilver.Text = "Кол-во девушек, сдавших на серебряный значок: " + _countQuantityGirlSilver;

                }
                if (dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Value.ToString() == "3")
                {
                    _countQuantityGirlBronze++;
                    lGirlBronze.Text = "Кол-во девушек, сдавших на бронзовый  значок: " + _countQuantityGirlBronze;

                }
                if (dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Value.ToString() == "0")
                {
                    _countQuantityGirlNotPassed++;
                    lGirlNotPassed.Text = "Кол-во девушек, не сдавших норму ГТО: " + _countQuantityGirlNotPassed;

                }
            }
            //подсчёт количества определённых мест


            chartGirl.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;// тип диаграммы

            chartGirl.Titles.Add("Для девушек по данному виду спорта");//название диаграммы

            if (_countQuantityGirlGold != 0)
            {
                chartGirl.Series[0].Points.AddY(_countQuantityGirlGold);//значениe
                chartGirl.Series[0].Points[0].Color = PieColors[0]; //цвет диаграммы
                chartGirl.Series[0].Points[0].LegendText = "Кол-во девушек, сдавших на золотой значок";//название подписи данных
            }
            if (_countQuantityGirlSilver != 0)
            {
                chartGirl.Series[0].Points.AddY(_countQuantityGirlSilver);// значениe
                chartGirl.Series[0].Points[1].Color = PieColors[1]; //цвет диаграммы
                chartGirl.Series[0].Points[1].LegendText = "Кол-во девушек, сдавших на серебряный значок";//название подписи данных
            }
            if (_countQuantityGirlBronze != 0)
            {
                chartGirl.Series[0].Points.AddY(_countQuantityGirlBronze);//значениe
                chartGirl.Series[0].Points[2].Color = PieColors[2]; //цвет диаграммы
                chartGirl.Series[0].Points[2].LegendText = "Кол-во девушек, сдавших на бронзовый  значок";//название подписи данных
            }
            if (_countQuantityGirlNotPassed != 0)
            {
                chartGirl.Series[0].Points.AddY(_countQuantityGirlNotPassed);//значениe
                chartGirl.Series[0].Points[3].Color = PieColors[3]; //цвет диаграммы
                chartGirl.Series[0].Points[3].LegendText = "Кол-во девушек, не сдавших норму ГТО";//название подписи данных
            }




            //подсчёт количества определённых мест
            #region подсчёт количества определённых мест
            _countQuantityManGold = 0;
            _countQuantityManSilver = 0;
            _countQuantityManBronze = 0;
            _countQuantityManNotPassed = 0;
            for (int i = 0; i < dgvExcel.RowCount - 1; i++)
            {

                if (dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Value.ToString() == "1")
                {
                    _countQuantityManGold++;
                    lManGold.Text = "Кол-во юношей, сдавших на золотой значок: " + _countQuantityManGold;

                }
                if (dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Value.ToString() == "2")
                {
                    _countQuantityManSilver++;
                    lManSilver.Text = "Кол-во юношей, сдавших на серебряный значок: " + _countQuantityManSilver;

                }
                if (dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Value.ToString() == "3")
                {
                    _countQuantityManBronze++;
                    lManBronze.Text = "Кол-во юношей, сдавших на бронзовый  значок: " + _countQuantityManBronze;

                }
                if (dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Value.ToString() == "0")
                {
                    _countQuantityManNotPassed++;
                    lManNotPassed.Text = "Кол-во юношей, не сдавших норму ГТО: " + _countQuantityManNotPassed;

                }
            }
            #endregion
            //подсчёт количества определённых мест


            chartMan.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;// тип диаграммы
            chartMan.Titles.Add("Для юношей по данному виду спорта");//название диаграммы

            if (_countQuantityManGold > 0)
            {
                chartMan.Series[0].Points.AddY(_countQuantityManGold);//значение
                chartMan.Series[0].Points[0].Color = PieColors[0]; //цвет диаграммы
                chartMan.Series[0].Points[0].LegendText = "Кол-во юношей, сдавших на золотой значок";//название подписи данных
            }
            if (_countQuantityManSilver > 0)
            {
                chartMan.Series[0].Points.AddY(_countQuantityManSilver);//значение
                chartMan.Series[0].Points[1].Color = PieColors[1]; //цвет диаграммы
                chartMan.Series[0].Points[1].LegendText = "Кол-во юношей, сдавших на серебряный значок";//название подписи данных
            }
            if (_countQuantityManBronze > 0)
            {
                chartMan.Series[0].Points.AddY(_countQuantityManBronze);//значение
                chartMan.Series[0].Points[2].Color = PieColors[2]; //цвет диаграммы
                chartMan.Series[0].Points[2].LegendText = "Кол-во юношей, сдавших на бронзовый  значок";//название подписи данных
            }
            if (_countQuantityManNotPassed > 0)
            {
                chartMan.Series[0].Points.AddY(_countQuantityManNotPassed);//значение
                chartMan.Series[0].Points[3].Color = PieColors[3]; //цвет диаграммы
                chartMan.Series[0].Points[3].LegendText = "Кол-во юношей, не сдавших норму ГТО";//название подписи данных
            }


            for (int i = 0; i < dgvExcel.RowCount - 1; i++)
            {

                if (dgvExcel.Rows[i].Cells["Занятое место в группе (Ранговая)"].Value.ToString() == "1")
                {
                    dgvExcel.Rows[i].Cells["Значок ГТО (золотой, серебряный,  бронзовый, нет) (Номинальная)"].Value = "Золотой";
                }
                if (dgvExcel.Rows[i].Cells["Занятое место в группе (Ранговая)"].Value.ToString() == "2")
                {
                    dgvExcel.Rows[i].Cells["Значок ГТО (золотой, серебряный,  бронзовый, нет) (Номинальная)"].Value = "Серебряный";
                }
                if (dgvExcel.Rows[i].Cells["Занятое место в группе (Ранговая)"].Value.ToString() == "3")
                {
                    dgvExcel.Rows[i].Cells["Значок ГТО (золотой, серебряный,  бронзовый, нет) (Номинальная)"].Value = "Бронзовый";
                }
                if (dgvExcel.Rows[i].Cells["Занятое место в группе (Ранговая)"].Value.ToString() == "0")
                {
                    dgvExcel.Rows[i].Cells["Значок ГТО (золотой, серебряный,  бронзовый, нет) (Номинальная)"].Value = "Нет";
                }

                dgvExcel.Rows[i].Cells["Зачет/незачет ( незачет, если нет значка) (Номинальная)"].Value = (dgvExcel.Rows[i].Cells["Значок ГТО (золотой, серебряный,  бронзовый, нет) (Номинальная)"].Value.ToString() == "Нет") ? "Незачёт" : "Зачёт";

            }



            //int lec = 0;
            //for (int i = 0; i < dgvExcel.RowCount; i++)
            //{
            //    //lec = lec + Convert.ToInt32(dgvExcel["Номер", i].Value);
            //    ////dataGridView2.Rows[0].Cells[0].Value = lec.ToString();
            //    //label1.Text = lec.ToString();
            //}
            Task_Load(sender, e);
        }


        private void Task_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvExcel.RowCount - 1; i++)
            {
                //если пустое поле место среди девушек
                dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].ReadOnly = (dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Value.ToString() == null || dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Value.ToString() == "") ? false : true;
                dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Style.BackColor = (dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Value.ToString() == null || dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Value.ToString() == "") ? Color.White : Color.Red;

                //если пустое поле место среди юношей
                dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].ReadOnly = (dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Value.ToString() == null || dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Value.ToString() == "") ? false : true;
                dgvExcel.Rows[i].Cells["Для девушек (занятое место среди девушек) (Ранговая)"].Style.BackColor = (dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Value.ToString() == null || dgvExcel.Rows[i].Cells["Для юношей (занятое место среди юношей) (Ранговая)"].Value.ToString() == "") ? Color.White : Color.Red;
            }


            btnResultCharts.Enabled = (dgvExcel.RowCount != 0 && (_countQuantityAllGold == 0 || _countQuantityAllSilver == 0 || _countQuantityAllBronze == 0 || _countQuantityAllNotPassed == 0)) ? true : false;// Тернарная условная операция
            btnClearCharts.Enabled = (_countQuantityAllGold != 0 || _countQuantityAllSilver != 0 || _countQuantityAllBronze != 0 || _countQuantityAllNotPassed != 0) ? true : false;// Тернарная условная операция

            //2 практическая работа
            LoadTwo();
            //2 практическая работа

            //3 практическая работа
            LoadThree();
            //3 практическая работа

            //4 практическая работа
            LoadFour();
            //4 практическая работа

            //6 практическая работа
            LoadSix();
            //6 практическая работа

            //8 практическая работа
            LoadEight();
            //8 практическая работа
        }

        private void btnClearCharts_Click(object sender, EventArgs e)
        {
            clearAll();
            Task_Load(sender, e);
        }



        private void clearAll()
        {
            chartAll.Series[0].Points.Clear(); //очистка диаграммы
            chartAll.Titles.Clear(); //очистка названия диаграммы
            chartGirl.Series[0].Points.Clear();
            chartGirl.Titles.Clear();
            chartMan.Series[0].Points.Clear();
            chartMan.Titles.Clear();
            _countQuantityAllGold = 0;
            _countQuantityAllSilver = 0;
            _countQuantityAllBronze = 0;
            _countQuantityAllNotPassed = 0;
            _countQuantityGirlGold = 0;
            _countQuantityGirlSilver = 0;
            _countQuantityGirlBronze = 0;
            _countQuantityGirlNotPassed = 0;
            _countQuantityManGold = 0;
            _countQuantityManSilver = 0;
            _countQuantityManBronze = 0;
            _countQuantityManNotPassed = 0;
        }

        //событие при изменении ячейки
        private void dgvExcel_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //dgvExcel.Rows[dgvExcel.RowCount - 2].Cells["Номер"].Value = dgvExcel.RowCount-1;

            clearAll();
            btnResultCharts_Click(sender, e);
        }



        //событие при изменении ячейки

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateForm createForm = new CreateForm();

            createForm.ShowDialog();
            if (createForm.DialogResult == DialogResult.OK)
            {
                //dgvExcel.Rows.Add("0","1","2","3","4","5", "6", "7", "8", "9", "10","11", "12");
                //dgvExcel.RowCount = dgvExcel.RowCount+ 1;
                dgvExcel.Rows[dgvExcel.RowCount - 1].Cells["Номер"].Value = TRPData.Id;

            }

        }



        //2 практическая работа
        //StreamWriter sW = new StreamWriter("res.txt"); //для сохранения в файл


        private void LoadTwo()
        {
            cbSignificance.SelectedIndex = 0;
            dgvPearsonX.Rows.Add();
            dgvPearsonX.Rows[1].Cells[0].Value = "zxi";
            dgvPearsonY.Rows.Add();
            dgvPearsonY.Rows[1].Cells[0].Value = "zyi";

            dgvPearsonX.Rows[0].Cells[1].Value = "75";
            dgvPearsonX.Rows[0].Cells[2].Value = "77";
            dgvPearsonX.Rows[0].Cells[3].Value = "76";
            dgvPearsonX.Rows[0].Cells[4].Value = "77";
            dgvPearsonX.Rows[0].Cells[5].Value = "76";
            dgvPearsonX.Rows[0].Cells[6].Value = "70";
            dgvPearsonX.Rows[0].Cells[7].Value = "70";
            dgvPearsonX.Rows[0].Cells[8].Value = "70";
            dgvPearsonX.Rows[0].Cells[9].Value = "69";
            dgvPearsonX.Rows[0].Cells[10].Value = "69";
            dgvPearsonX.Rows[0].Cells[11].Value = "67";
            dgvPearsonX.Rows[0].Cells[12].Value = "67";
            dgvPearsonX.Rows[0].Cells[13].Value = "67";
            dgvPearsonX.Rows[0].Cells[14].Value = "68";
            dgvPearsonX.Rows[0].Cells[15].Value = "67";
            dgvPearsonX.Rows[0].Cells[16].Value = "67";
            dgvPearsonX.Rows[0].Cells[17].Value = "67";
            dgvPearsonX.Rows[0].Cells[18].Value = "65";
            dgvPearsonX.Rows[0].Cells[19].Value = "65";
            dgvPearsonX.Rows[0].Cells[20].Value = "67";



            dgvPearsonY.Rows[0].Cells[1].Value = "125";
            dgvPearsonY.Rows[0].Cells[2].Value = "125";
            dgvPearsonY.Rows[0].Cells[3].Value = "124";
            dgvPearsonY.Rows[0].Cells[4].Value = "123";
            dgvPearsonY.Rows[0].Cells[5].Value = "122";
            dgvPearsonY.Rows[0].Cells[6].Value = "123";
            dgvPearsonY.Rows[0].Cells[7].Value = "122";
            dgvPearsonY.Rows[0].Cells[8].Value = "122";
            dgvPearsonY.Rows[0].Cells[9].Value = "122";
            dgvPearsonY.Rows[0].Cells[10].Value = "120";
            dgvPearsonY.Rows[0].Cells[11].Value = "120";
            dgvPearsonY.Rows[0].Cells[12].Value = "120";
            dgvPearsonY.Rows[0].Cells[13].Value = "120";
            dgvPearsonY.Rows[0].Cells[14].Value = "119";
            dgvPearsonY.Rows[0].Cells[15].Value = "118";
            dgvPearsonY.Rows[0].Cells[16].Value = "117";
            dgvPearsonY.Rows[0].Cells[17].Value = "118";
            dgvPearsonY.Rows[0].Cells[18].Value = "117";
            dgvPearsonY.Rows[0].Cells[19].Value = "118";
            dgvPearsonY.Rows[0].Cells[20].Value = "116";
        }



        private void ChangeableDataX()
        {
            dgvPearsonX.RowCount = 7;
            dgvPearsonX.Rows[0].Cells[0].Value = "xi";
            dgvPearsonX.Rows[1].Cells[0].Value = "zxi";
            dgvPearsonX.Rows[2].Cells[0].Value = "xi*";
            dgvPearsonX.Rows[3].Cells[0].Value = "zi";
            dgvPearsonX.Rows[4].Cells[0].Value = "Ф(zi)";
            dgvPearsonX.Rows[5].Cells[0].Value = "pi";
            dgvPearsonX.Rows[6].Cells[0].Value = "ni'";


            for (int i = 1; i < dgvPearsonX.Columns.Count; i++)
            {
                if (dgvPearsonX.Rows[0].Cells[i].Value != null && dgvPearsonX.Rows[0].Cells[i].Value.ToString() != "" && tbM.Text != "" && tbM.Text != null)
                {
                    dgvPearsonX.Rows[1].Cells[i].Value = Math.Round((Convert.ToDouble(dgvPearsonX.Rows[0].Cells[i].Value) - Convert.ToDouble(tbM.Text)) / Convert.ToDouble(tbM.Text), 3);


                }
                else
                {
                    dgvPearsonX.Rows[1].Cells[i].Value = "";
                    dgvPearsonX.Rows[2].Cells[i].Value = "";
                    dgvPearsonX.Rows[3].Cells[i].Value = "";
                    dgvPearsonX.Rows[4].Cells[i].Value = "";
                    dgvPearsonX.Rows[5].Cells[i].Value = "";
                    dgvPearsonX.Rows[6].Cells[i].Value = "";
                }
            }
        }
        private void ChangeableDataY()
        {
            dgvPearsonY.RowCount = 7;
            dgvPearsonY.Rows[0].Cells[0].Value = "yi";
            dgvPearsonY.Rows[1].Cells[0].Value = "zyi";
            dgvPearsonY.Rows[2].Cells[0].Value = "yi*";
            dgvPearsonY.Rows[3].Cells[0].Value = "zi";
            dgvPearsonY.Rows[4].Cells[0].Value = "Ф(zi)";
            dgvPearsonY.Rows[5].Cells[0].Value = "pi";
            dgvPearsonY.Rows[6].Cells[0].Value = "ni'";

            for (int i = 1; i < dgvPearsonY.Columns.Count; i++)
            {
                if (dgvPearsonY.Rows[0].Cells[i].Value != null && dgvPearsonY.Rows[0].Cells[i].Value.ToString() != "" && tbN.Text != "" && tbN.Text != null)
                {
                    dgvPearsonY.Rows[1].Cells[i].Value = Math.Round((Convert.ToDouble(dgvPearsonY.Rows[0].Cells[i].Value) - Convert.ToDouble(tbN.Text)) / Convert.ToDouble(tbN.Text), 3);
                }
                else
                {
                    dgvPearsonY.Rows[1].Cells[i].Value = "";
                    dgvPearsonY.Rows[2].Cells[i].Value = "";
                    dgvPearsonY.Rows[3].Cells[i].Value = "";
                    dgvPearsonY.Rows[4].Cells[i].Value = "";
                    dgvPearsonY.Rows[5].Cells[i].Value = "";
                    dgvPearsonY.Rows[6].Cells[i].Value = "";
                }
            }
        }




        private void tbM_TextChanged(object sender, EventArgs e)
        {
            ChangeableDataX();
        }


        private void dgvPearsonX_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ChangeableDataX();
        }

        private void tbN_TextChanged(object sender, EventArgs e)
        {
            ChangeableDataY();
        }

        private void dgvPearsonY_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ChangeableDataY();
        }


        //событие на выбор уровня значимости а
        private void cbSignificance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lComparisonX.Text != null && lComparisonX.Text != "")
            {
                btnResult_Click(sender, e);
            }
        }
        //событие на выбор уровня значимости а



        private void btnResult_Click(object sender, EventArgs e)
        {
            double sumOneX = 0;
            double sumTwoX = 0;
            double sigmaX;
            double zX;
            double observedX = 0;
            int k = 5 - 3;
            double criticalPointX = 0;
            double underlineX = 0;

            double sumOneY = 0;
            double sumTwoY = 0;
            double sigmaY;
            double zY;
            double observedY = 0;
            int kY = 5 - 3;
            double criticalPointY = 0;
            double underlineY = 0;


            //таблица значений интегральной функции Лапласа
            var fLaplace = new Dictionary<double, double>()
            {{
0.000,0.0000},
{ 0.010,0.0040},
{ 0.020,0.0080},
{ 0.030,0.0120},
{ 0.040,0.0160},
{ 0.050,0.0199},
{ 0.060,0.0239},
{ 0.070,0.0279},
{ 0.080,0.0319},
{ 0.090,0.0359},
{ 0.100,0.0398},
{ 0.110,0.0438},
{ 0.120,0.0478},
{ 0.130,0.0517},
{ 0.140,0.0557},
{ 0.150,0.0596},
{ 0.160,0.0636},
{ 0.170,0.0675},
{ 0.180,0.0714},
{ 0.190,0.0753},
{ 0.200,0.0793},
{ 0.210,0.0832},
{ 0.220,0.0871},
{ 0.230,0.0910},
{ 0.240,0.0948},
{ 0.250,0.0987},
{ 0.260,0.1026},
{ 0.270,0.1064},
{ 0.280,0.1103},
{ 0.290,0.1141},
{ 0.300,0.1179},
{ 0.310,0.1217},
{ 0.320,0.1255},
{ 0.330,0.1293},
{ 0.340,0.1331},
{ 0.350,0.1368},
{ 0.360,0.1406},
{ 0.370,0.1443},
{ 0.380,0.1480},
{ 0.390,0.1517},
{ 0.400,0.1554},
{ 0.410,0.1591},
{ 0.420,0.1628},
{ 0.430,0.1664},
{ 0.440,0.1700},
{ 0.450,0.1736},
{ 0.460,0.1772},
{ 0.470,0.1808},
{ 0.480,0.1844},
{ 0.490,0.1879},
{ 0.500,0.1915},
{ 0.510,0.1950},
{ 0.520,0.1985},
{ 0.530,0.2019},
{ 0.540,0.2054},
{ 0.550,0.2088},
{ 0.560,0.2123},
{ 0.570,0.2157},
{ 0.580,0.2190},
{ 0.590,0.2224},
{ 0.600,0.2257},
{ 0.610,0.2291},
{ 0.620,0.2324},
{ 0.630,0.2357},
{ 0.640,0.2389},
{ 0.650,0.2422},
{ 0.660,0.2454},
{ 0.670,0.2486},
{ 0.680,0.2517},
{ 0.690,0.2549},
{ 0.700,0.2580},
{ 0.710,0.2611},
{ 0.720,0.2642},
{ 0.730,0.2673},
{ 0.740,0.2703},
{ 0.750,0.2734},
{ 0.760,0.2764},
{ 0.770,0.2794},
{ 0.780,0.2823},
{ 0.790,0.2852},
{ 0.800,0.2881},
{ 0.810,0.2910},
{ 0.820,0.2939},
{ 0.830,0.2967},
{ 0.840,0.2995},
{ 0.850,0.3023},
{ 0.860,0.3051},
{ 0.870,0.3078},
{ 0.880,0.3106},
{ 0.890,0.3133},
{ 0.900,0.3159},
{ 0.910,0.3186},
{ 0.920,0.3212},
{ 0.930,0.3238},
{ 0.940,0.3264},
{ 0.950,0.3289},
{ 0.960,0.3315},
{ 0.970,0.3340},
{ 0.980,0.3365},
{ 0.990,0.3389},
{ 1.000,0.3413},
{1.010,0.3438},
{1.020,0.3461},
{1.030,0.3485},
{1.040,0.3508},
{1.050,0.3531},
{1.060,0.3554},
{1.070,0.3577},
{1.080,0.3599},
{1.090,0.3621},
{1.100,0.3643},
{1.110,0.3665},
{1.120,0.3686},
{1.130,0.3708},
{1.140,0.3729},
{1.150,0.3749},
{1.160,0.3770},
{1.170,0.3790},
{1.180,0.3810},
{1.190,0.3830},
{1.200,0.3849},
{1.210,0.3869},
{1.220,0.3883},
{1.230,0.3907},
{1.240,0.3925},
{1.250,0.3944},
{1.260,0.3962},
{1.270,0.3980},
{1.280,0.3997},
{1.290,0.4015},
{1.300,0.4032},
{1.310,0.4049},
{1.320,0.4066},
{1.330,0.4082},
{1.340,0.4099},
{1.350,0.4115},
{1.360,0.4131},
{1.370,0.4147},
{1.380,0.4162},
{1.390,0.4177},
{1.400,0.4192},
{1.410,0.4207},
{1.420,0.4222},
{1.430,0.4236},
{1.440,0.4251},
{1.450,0.4265},
{1.460,0.4279},
{1.470,0.4292},
{1.480,0.4306},
{1.490,0.4319},
{1.500,0.4332},
{1.510,0.4345},
{1.520,0.4357},
{1.530,0.4370},
{1.540,0.4382},
{1.550,0.4394},
{1.560,0.4406},
{1.570,0.4418},
{1.580,0.4429},
{1.590,0.4441},
{1.600,0.4452},
{1.610,0.4463},
{1.620,0.4474},
{1.630,0.4484},
{1.640,0.4495},
{1.650,0.4505},
{1.660,0.4515},
{1.670,0.4525},
{1.680,0.4535},
{1.690,0.4545},
{1.700,0.4554},
{1.710,0.4564},
{1.720,0.4573},
{1.730,0.4582},
{1.740,0.4591},
{1.750,0.4599},
{1.760,0.4608},
{1.770,0.4616},
{1.780,0.4625},
{1.790,0.4633},
{1.800,0.4641},
{1.810,0.4649},
{1.820,0.4656},
{1.830,0.4664},
{1.840,0.4671},
{1.850,0.4678},
{1.860,0.4686},
{1.870,0.4693},
{1.880,0.4699},
{1.890,0.4706},
{1.900,0.4713},
{1.910,0.4719},
{1.920,0.4726},
{1.930,0.4732},
{1.940,0.4738},
{1.950,0.4744},
{1.960,0.4750},
{1.970,0.4756},
{1.980,0.4761},
{1.990,0.4767},
{2.000,0.4772},
{2.020,0.4783},
{2.040,0.4793},
{2.060,0.4803},
{2.080,0.4812},
{2.100,0.4821},
{2.120,0.4830},
{2.140,0.4838},
{2.160,0.4846},
{2.180,0.4854},
{2.200,0.4861},
{2.220,0.4868},
{2.240,0.4875},
{2.260,0.4881},
{2.280,0.4887},
{2.300,0.4893},
{2.320,0.4898},
{2.340,0.4904},
{2.360,0.4909},
{2.380,0.4913},
{2.400,0.4918},
{2.420,0.4922},
{2.440,0.4927},
{2.460,0.4931},
{2.480,0.4934},
{2.500,0.4938},
{2.520,0.4941},
{2.540,0.4945},
{2.560,0.4948},
{2.580,0.4951},
{2.600,0.4953},
{2.620,0.4956},
{2.640,0.4959},
{2.660,0.4961},
{2.680,0.4963},
                        { 2.700, 0.4965},
                        { 2.720, 0.4967},
                        { 2.740, 0.4969},
                        { 2.760, 0.4971},
                        { 2.780, 0.4973},
                        { 2.800, 0.4974},
                        { 2.820, 0.4976},
                        { 2.840, 0.4977},
                        { 2.860, 0.4979},
                        { 2.880, 0.4980},
                        { 2.900, 0.4981},
                        { 2.920, 0.4982},
                        { 2.940, 0.4984},
                        { 2.960, 0.4985},
                        { 2.980, 0.4986},
                        { 3.000, 0.49865},
                        { 3.200, 0.499931},
                        { 3.400, 0.49966},
                        { 3.600, 0.499841},
                        { 3.800, 0.499928},
                        { 4.000, 0.499968},
                        { 4.500, 0.499997 },
                        { 5.000, 0.499997 },

            };
            //таблица значений интегральной функции Лапласа



            //расчёт для x
            for (int i = 1; i < dgvPearsonX.Columns.Count - 1; i++)
            {

                if (dgvPearsonX.Rows[0].Cells[i].Value != null && dgvPearsonX.Rows[0].Cells[i].Value.ToString() != "" && tbM.Text != "" && tbM.Text != null)
                {
                    //xi* формула
                    dgvPearsonX.Rows[2].Cells[i].Value = Math.Round((Convert.ToDouble(dgvPearsonX.Rows[0].Cells[i].Value) - Convert.ToDouble(dgvPearsonX.Rows[0].Cells[i + 1].Value)) / 2, 3);
                    //xi* формула

                    //¯x формула
                    sumOneX += Convert.ToDouble(dgvPearsonX[i, 2].Value);
                    underlineX = /*"¯x = " + */((double)1 / 20 * sumOneX)/*.ToString()*/;
                    //¯x = 0,2 формула

                    //формула сигма σ 
                    sumTwoX += Math.Pow(Convert.ToDouble(dgvPearsonX[i, 0].Value) - underlineX, 2);
                    sigmaX = Math.Pow(Convert.ToDouble((double)1 / 20 * sumTwoX), 0.5);
                    //формула сигма σ = 68,15


                    zX = Convert.ToDouble((20 - underlineX) / sigmaX); //формула Z

                    //формула zi
                    dgvPearsonX.Rows[3].Cells[i].Value = Math.Round((Convert.ToDouble(dgvPearsonX.Rows[0].Cells[i].Value) - underlineX) / sigmaX, 3);
                    //формула zi

                    //Вывод значений функции Лапласа
                    foreach (var fL in fLaplace)
                    {
                        if (fL.Key <= Convert.ToDouble(dgvPearsonX.Rows[3].Cells[i].Value))
                        {
                            dgvPearsonX.Rows[4].Cells[i].Value = fL.Value;
                        }

                    }
                    //Вывод значений функции Лапласа

                    //double[,] numbers = { { 6.6, 5.0, 3.8, 0.0039, 0.00098, 0.00016 },
                    //                      { 9.2, 7.4, 6.0, 0.103, 0.051, 0.020 } };



                }
                else
                {
                    dgvPearsonX.Rows[1].Cells[i].Value = "";
                    dgvPearsonX.Rows[2].Cells[i].Value = "";
                    dgvPearsonX.Rows[3].Cells[i].Value = "";
                    dgvPearsonX.Rows[4].Cells[i].Value = "";
                    dgvPearsonX.Rows[5].Cells[i].Value = "";
                    dgvPearsonX.Rows[6].Cells[i].Value = "";
                }
            }
            //расчёт для x


            //List<MappedKey>criticalPoints = new List<MappedKey>();
            //criticalPoints.Add(new MappedKey(12.2,14.3,4.2));

            //критические точки распределения X^2
            if (cbSignificance.Text == "0.01" && k == 1)
            {
                criticalPointX = 6.6;
                lСriticalX.Text = "X^2кр = " + 6.6;
                criticalPointY = 6.6;
                lСriticalY.Text = "Y^2кр = " + 6.6;
            }
            if (cbSignificance.Text == "0.01" && k == 2)
            {
                criticalPointX = 9.2;
                lСriticalX.Text = "X^2кр = " + 9.2;
                criticalPointY = 9.2;
                lСriticalY.Text = "Y^2кр = " + 9.2;
            }
            if (cbSignificance.Text == "0.025" && k == 1)
            {
                criticalPointX = 5.0;
                lСriticalX.Text = "X^2кр = " + 5.0;
                criticalPointY = 5.0;
                lСriticalY.Text = "Y^2кр = " + 5.0;
            }
            if (cbSignificance.Text == "0.025" && k == 2)
            {
                criticalPointX = 7.4;
                lСriticalX.Text = "X^2кр = " + 7.4;
                criticalPointY = 7.4;
                lСriticalY.Text = "Y^2кр = " + 7.4;
            }
            if (cbSignificance.Text == "0.05" && k == 1)
            {
                criticalPointX = 3.8;
                lСriticalX.Text = "X^2кр = " + 3.8;
                criticalPointY = 3.8;
                lСriticalY.Text = "Y^2кр = " + 3.8;
            }
            if (cbSignificance.Text == "0.05" && k == 2)
            {
                criticalPointX = 6.0;
                lСriticalX.Text = "X^2кр = " + 6.0;
                criticalPointY = 6.0;
                lСriticalY.Text = "Y^2кр = " + 6.0;
            }
            if (cbSignificance.Text == "0.95" && k == 1)
            {
                criticalPointX = 0.0039;
                lСriticalX.Text = "X^2кр = " + 0.0039;
                criticalPointY = 0.0039;
                lСriticalY.Text = "Y^2кр = " + 0.0039;
            }
            if (cbSignificance.Text == "0.95" && k == 2)
            {
                criticalPointX = 0.103;
                lСriticalX.Text = "X^2кр = " + 0.103;
                criticalPointY = 0.103;
                lСriticalY.Text = "Y^2кр = " + 0.103;
            }
            if (cbSignificance.Text == "0.975" && k == 1)
            {
                criticalPointX = 0.00098;
                lСriticalX.Text = "X^2кр = " + 0.00098;
                criticalPointY = 0.00098;
                lСriticalY.Text = "Y^2кр = " + 0.00098;
            }
            if (cbSignificance.Text == "0.975" && k == 2)
            {
                criticalPointX = 0.051;
                lСriticalX.Text = "X^2кр = " + 0.051;
                criticalPointY = 0.051;
                lСriticalY.Text = "Y^2кр = " + 0.051;
            }
            if (cbSignificance.Text == "0.89" && k == 1)
            {
                criticalPointX = 0.00016;
                lСriticalX.Text = "X^2кр = " + 0.00016;
                criticalPointY = 0.00016;
                lСriticalY.Text = "Y^2кр = " + 0.00016;
            }
            if (cbSignificance.Text == "0.89" && k == 2)
            {
                criticalPointX = 0.020;
                lСriticalX.Text = "X^2кр = " + 0.020;
                criticalPointY = 0.020;
                lСriticalY.Text = "Y^2кр = " + 0.020;
            }
            if (cbSignificance.Text == "0.01" && k == 3)
            {
                criticalPointX = 11.3;
                lСriticalX.Text = "X^2кр = " + 11.3;
                criticalPointY = 11.3;
                lСriticalY.Text = "Y^2кр = " + 11.3;
            }
            if (cbSignificance.Text == "0.025" && k == 3)
            {
                criticalPointX = 9.4;
                lСriticalX.Text = "X^2кр = " + 9.4;
                criticalPointY = 9.4;
                lСriticalY.Text = "Y^2кр = " + 9.4;
            }
            if (cbSignificance.Text == "0.05" && k == 3)
            {
                criticalPointX = 7.8;
                lСriticalX.Text = "X^2кр = " + 7.8;
                criticalPointY = 7.8;
                lСriticalY.Text = "Y^2кр = " + 7.8;
            }
            if (cbSignificance.Text == "0.95" && k == 3)
            {
                criticalPointX = 0.352;
                lСriticalX.Text = "X^2кр = " + 0.352;
                criticalPointY = 0.352;
                lСriticalY.Text = "Y^2кр = " + 0.352;
            }
            if (cbSignificance.Text == "0.975" && k == 3)
            {
                criticalPointX = 0.216;
                lСriticalX.Text = "X^2кр = " + 0.216;
                criticalPointY = 0.216;
                lСriticalY.Text = "Y^2кр = " + 0.216;
            }
            if (cbSignificance.Text == "0.89" && k == 3)
            {
                criticalPointX = 0.115;
                lСriticalX.Text = "X^2кр = " + 0.115;
                criticalPointY = 0.115;
                lСriticalY.Text = "Y^2кр = " + 0.115;
            }
            //критические точки распределения X^2

            for (int i = 1; i < dgvPearsonX.Columns.Count - 2; i++)
            {
                //pi формула
                dgvPearsonX.Rows[5].Cells[i].Value = Math.Round(Convert.ToDouble(dgvPearsonX.Rows[4].Cells[i + 1].Value) - Convert.ToDouble(dgvPearsonX.Rows[4].Cells[1].Value), 6);
                //pi формула

                //ni' формула
                dgvPearsonX.Rows[6].Cells[i].Value = Math.Round(Convert.ToDouble(dgvPearsonX.Rows[5].Cells[i].Value) * 20, 6);
                //ni' формула

                observedX /*+*/= Math.Round(Math.Pow(20 - Convert.ToDouble(dgvPearsonX.Rows[6].Cells[i].Value), 2) / Convert.ToDouble(dgvPearsonX.Rows[6].Cells[i].Value), 6);


                lDegreeFreedomKX.Text = "Число степеней свободы k= " + (5 - 3).ToString();
                lObservedX.Text = "X^2набл = " + Math.Round(Math.Sqrt(Math.Abs(observedX)), 3);

            }

            //сравнение χ^2набл и χ^2кр
            lComparisonX.Text = (Math.Round(Math.Sqrt(Math.Abs(observedX)), 3) < criticalPointX) ? "χ^2набл < χ^2кр, нет оснований отвергнуть гипотезу Н0" : "χ^2набл > χ^2кр, гипотеза Н0 отвергается";
            //сравнение χ^2набл и χ^2кр


            //для сохранения в файл
            //for (double i = 0.000; i < 50; i += 0.010)
            //{
            //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            //    sW.WriteLine("{" + Math.Round(0.000 + i, 3).ToString("0.000") + "," + "0." + "}" + ",");
            //}
            //sW.Close();
            //для сохранения в файл


            //расчёт для y
            for (int i = 1; i < dgvPearsonY.Columns.Count - 1; i++)
            {

                if (dgvPearsonY.Rows[0].Cells[i].Value != null && dgvPearsonY.Rows[0].Cells[i].Value.ToString() != "" && tbN.Text != "" && tbN.Text != null)
                {
                    //yi* формула
                    dgvPearsonY.Rows[2].Cells[i].Value = Math.Round((Convert.ToDouble(dgvPearsonY.Rows[0].Cells[i].Value) - Convert.ToDouble(dgvPearsonY.Rows[0].Cells[i + 1].Value)) / 2, 3);
                    //yi* формула

                    //¯y формула
                    sumOneY += Convert.ToDouble(dgvPearsonY[i, 2].Value);
                    underlineY = ((double)1 / 20 * sumOneY);
                    //¯y 

                    //формула сигма σ 
                    sumTwoY += Math.Pow(Convert.ToDouble(dgvPearsonY[i, 0].Value) - underlineY, 2);
                    sigmaY = Math.Pow(Convert.ToDouble((double)1 / 20 * sumTwoY), 0.5);
                    //формула сигма σ


                    zY = Convert.ToDouble((20 - underlineY) / sigmaY); //формула Z

                    //формула zi
                    dgvPearsonY.Rows[3].Cells[i].Value = Math.Round((Convert.ToDouble(dgvPearsonY.Rows[0].Cells[i].Value) - underlineY) / sigmaY, 3);
                    //формула zi

                    //Вывод значений функции Лапласа
                    foreach (var fL in fLaplace)
                    {
                        if (fL.Key <= Convert.ToDouble(dgvPearsonY.Rows[3].Cells[i].Value))
                        {
                            dgvPearsonY.Rows[4].Cells[i].Value = fL.Value;
                        }

                    }
                    //Вывод значений функции Лапласа
                }
                else
                {
                    dgvPearsonY.Rows[1].Cells[i].Value = "";
                    dgvPearsonY.Rows[2].Cells[i].Value = "";
                    dgvPearsonY.Rows[3].Cells[i].Value = "";
                    dgvPearsonY.Rows[4].Cells[i].Value = "";
                    dgvPearsonY.Rows[5].Cells[i].Value = "";
                    dgvPearsonY.Rows[6].Cells[i].Value = "";
                }
            }

            for (int i = 1; i < dgvPearsonY.Columns.Count - 2; i++)
            {
                //pi формула
                dgvPearsonY.Rows[5].Cells[i].Value = Math.Round(Convert.ToDouble(dgvPearsonY.Rows[4].Cells[i + 1].Value) - Convert.ToDouble(dgvPearsonY.Rows[4].Cells[1].Value), 6);
                //pi формула

                //ni' формула
                dgvPearsonY.Rows[6].Cells[i].Value = Math.Round(Convert.ToDouble(dgvPearsonY.Rows[5].Cells[i].Value) * 20, 6);
                //ni' формула

                observedY /*+*/= Math.Round(Math.Pow(20 - Convert.ToDouble(dgvPearsonY.Rows[6].Cells[i].Value), 2) / Convert.ToDouble(dgvPearsonY.Rows[6].Cells[i].Value), 6);


                lDegreeFreedomKY.Text = "Число степеней свободы k= " + (5 - 3).ToString();
                lObservedY.Text = "Y^2набл = " + Math.Round(Math.Sqrt(Math.Abs(observedY)), 3);

            }

            //сравнение Y^2набл и Y^2кр
            lComparisonY.Text = (Math.Round(Math.Sqrt(Math.Abs(observedY)), 3) < criticalPointY) ? "Y^2набл < Y^2кр, нет оснований отвергнуть гипотезу Н0" : "Y^2набл > Y^2кр, гипотеза Н0 отвергается";
            //сравнение Y^2набл и Y^2кр
            //расчёт для y



            //Сравнение двух средних произвольно распределённых генеральных совокупностей
            double sumVarianceX = 0, varianceX, sumVarianceY = 0, varianceY, сomparisonZ, observedZ,
                underlineObservedX, underlineObservedY, underlineObservedSummX = 0, underlineObservedSummY = 0,
                functionLaplaceA, functionLaplaceB, criticalPointZa = 0, criticalPointZb = 0;

            for (int i = 1; i < dgvPearsonX.Columns.Count; i++)
            {
                //формула дисперсии D(X)
                sumVarianceX += Math.Pow(Convert.ToDouble(dgvPearsonX[i, 0].Value) - underlineX, 2);
                varianceX = Convert.ToDouble(1 / double.Parse(tbM.Text) * sumVarianceX);
                //формула дисперсии D(X)

                //формула дисперсии D(Y)
                sumVarianceY += Math.Pow(Convert.ToDouble(dgvPearsonY[i, 0].Value) - underlineY, 2);
                varianceY = Convert.ToDouble(1 / double.Parse(tbN.Text) * sumVarianceY);
                //формула дисперсии D(Y)
                //формула Z
                сomparisonZ = (underlineX - underlineY) / (Math.Sqrt((varianceX / double.Parse(tbM.Text)) + (varianceY / double.Parse(tbN.Text))));
                //формула Z

                //формула Z наблюдаемое
                underlineObservedSummX += Convert.ToDouble(dgvPearsonX[i, 0].Value);
                underlineObservedX = (1 / double.Parse(tbM.Text)) * underlineObservedSummX;
                underlineObservedSummY += Convert.ToDouble(dgvPearsonY[i, 0].Value);
                underlineObservedY = (1 / double.Parse(tbN.Text)) * underlineObservedSummX;
                observedZ = (underlineObservedX - underlineObservedY) / (Math.Sqrt((varianceX / double.Parse(tbM.Text)) + (varianceY / double.Parse(tbN.Text))));
                lObservedZ.Text = "Z^2набл = " + Math.Round(observedZ, 2);
                //формула Z наблюдаемое

                //для случая а
                //Zкр находится из условия Ф(Zкр)
                functionLaplaceA = (1 - Double.Parse(cbSignificance.Text, CultureInfo.InvariantCulture)) / 2;
                //Zкр находится из условия Ф(Zкр) 


                //Вывод значений функции Лапласа (Z наблюдаемое)
                foreach (var fL in fLaplace)
                {
                    if (fL.Value <= functionLaplaceA)
                    {
                        lCriticalZa.Text = "Z^2кр = " + (fL.Key).ToString();
                        criticalPointZa = fL.Key;
                    }
                }
                //Вывод значений функции Лапласа (Z наблюдаемое)


                //сравнение Z^2набл и Z^2кр
                lComparisonZa.Text = (Math.Round(Math.Abs(observedZ), 2) < criticalPointZa) ? "Z^2набл < Z^2кр, нет оснований отвергнуть гипотезу Н0" : "Z^2набл > Z^2кр, гипотеза Н0 отвергается";
                //сравнение Z^2набл и Z^2кр
                //для случая а



                //для случая b
                //Zкр находится из условия Ф(Zкр)
                functionLaplaceB = (1 - (2 * Double.Parse(cbSignificance.Text, CultureInfo.InvariantCulture))) / 2;
                //Zкр находится из условия Ф(Zкр) 

                //Вывод значений функции Лапласа (Z наблюдаемое)
                foreach (var fL in fLaplace)
                {
                    if (fL.Value <= Math.Abs(functionLaplaceB))
                    {
                        lCriticalZb.Text = "Z^2кр = " + (fL.Key).ToString();
                        criticalPointZb = fL.Key;
                    }
                }
                //Вывод значений функции Лапласа (Z наблюдаемое)


                //сравнение Z^2набл и Z^2кр
                lComparisonZb.Text = (Math.Round(Math.Abs(observedZ), 2) < criticalPointZb) ? "Z^2набл < Z^2кр, нет оснований отвергнуть гипотезу Н0" : "Z^2набл > Z^2кр, гипотеза Н0 отвергается";
                //сравнение Z^2набл и Z^2кр
                //для случая b
            }

            //Сравнение двух средних произвольно распределённых генеральных совокупностей

        }
        //2 практическая работа



        //3 практическая работа
        private void LoadThree()
        {

            dgvTablesValues.RowCount = 14;
            for (int i = 0; i < 10; ++i)
            {
                dgvTablesValues.Rows[i].Cells[0].Value = i + 1;
            }


            dgvTablesValues.Rows[0].Cells[1].Value = "550";
            dgvTablesValues.Rows[0].Cells[2].Value = "530";
            dgvTablesValues.Rows[0].Cells[3].Value = "520";
            dgvTablesValues.Rows[0].Cells[4].Value = "470";
            dgvTablesValues.Rows[0].Cells[5].Value = "436";

            dgvTablesValues.Rows[1].Cells[1].Value = "460";
            dgvTablesValues.Rows[1].Cells[2].Value = "536";
            dgvTablesValues.Rows[1].Cells[3].Value = "558";
            dgvTablesValues.Rows[1].Cells[4].Value = "501";
            dgvTablesValues.Rows[1].Cells[5].Value = "550";

            dgvTablesValues.Rows[2].Cells[1].Value = "510";
            dgvTablesValues.Rows[2].Cells[2].Value = "430";
            dgvTablesValues.Rows[2].Cells[3].Value = "524";
            dgvTablesValues.Rows[2].Cells[4].Value = "564";
            dgvTablesValues.Rows[2].Cells[5].Value = "534";

            dgvTablesValues.Rows[3].Cells[1].Value = "530";
            dgvTablesValues.Rows[3].Cells[2].Value = "480";
            dgvTablesValues.Rows[3].Cells[3].Value = "505";
            dgvTablesValues.Rows[3].Cells[4].Value = "465";
            dgvTablesValues.Rows[3].Cells[5].Value = "545";

            //
            dgvTablesValues.Rows[4].Cells[1].Value = "520";
            dgvTablesValues.Rows[4].Cells[2].Value = "523";
            dgvTablesValues.Rows[4].Cells[3].Value = "560";
            dgvTablesValues.Rows[4].Cells[4].Value = "452";
            dgvTablesValues.Rows[4].Cells[5].Value = "587";


            dgvTablesValues.Rows[5].Cells[1].Value = "537";
            dgvTablesValues.Rows[5].Cells[2].Value = "528";
            dgvTablesValues.Rows[5].Cells[3].Value = "542";
            dgvTablesValues.Rows[5].Cells[4].Value = "605";
            dgvTablesValues.Rows[5].Cells[5].Value = "587";

            dgvTablesValues.Rows[6].Cells[1].Value = "520";
            dgvTablesValues.Rows[6].Cells[2].Value = "590";
            dgvTablesValues.Rows[6].Cells[3].Value = "512";
            dgvTablesValues.Rows[6].Cells[4].Value = "475";
            dgvTablesValues.Rows[6].Cells[5].Value = "546";

            dgvTablesValues.Rows[7].Cells[1].Value = "576";
            dgvTablesValues.Rows[7].Cells[2].Value = "605";
            dgvTablesValues.Rows[7].Cells[3].Value = "532";
            dgvTablesValues.Rows[7].Cells[4].Value = "524";
            dgvTablesValues.Rows[7].Cells[5].Value = "490";

            dgvTablesValues.Rows[8].Cells[1].Value = "556";
            dgvTablesValues.Rows[8].Cells[2].Value = "532";
            dgvTablesValues.Rows[8].Cells[3].Value = "560";
            dgvTablesValues.Rows[8].Cells[4].Value = "561";
            dgvTablesValues.Rows[8].Cells[5].Value = "587";

            dgvTablesValues.Rows[9].Cells[1].Value = "530";
            dgvTablesValues.Rows[9].Cells[2].Value = "561";
            dgvTablesValues.Rows[9].Cells[3].Value = "521";
            dgvTablesValues.Rows[9].Cells[4].Value = "532";
            dgvTablesValues.Rows[9].Cells[5].Value = "578";


            dgvTablesValues.Rows[10].Cells[0].Value = "x ̅_j";
            dgvTablesValues.Rows[11].Cells[0].Value = "S_j^2";
            dgvTablesValues.Rows[12].Cells[0].Value = "s_j^2";
            dgvTablesValues.Rows[13].Cells[0].Value = "x ̅";


            for (int i = 0; i < dgvTablesValues.Columns.Count; i++)
            {
                for (int j = 10; j < dgvTablesValues.RowCount; j++)
                {
                    dgvTablesValues.Rows[j].Cells[i].ReadOnly = true; //запрет на запись
                    dgvTablesValues.Rows[j].Cells[i].Style.BackColor = Color.GreenYellow;//цвет ячеек
                }
            }
            for (int j = 0; j < dgvTablesValues.RowCount; j++)
            {
                dgvTablesValues.Rows[j].Cells[0].ReadOnly = true;//запрет на запись
            }

        }

        private void btnResultV_Click(object sender, EventArgs e)
        {
            double[] sumFp = new double[6];
            double[] variance = new double[6];
            //расчёт среднего значения Fi, x ̅_j
            for (int i = 0; i < dgvTablesValues.Rows.Count - 4; i++)
            {

                if (dgvTablesValues.Rows[i].Cells[1].Value != null && dgvTablesValues.Rows[i].Cells[1].Value.ToString() != "")
                {

                    sumFp[1] += Convert.ToDouble(dgvTablesValues[1, i].Value);
                    sumFp[2] += Convert.ToDouble(dgvTablesValues[2, i].Value);
                    sumFp[3] += Convert.ToDouble(dgvTablesValues[3, i].Value);
                    sumFp[4] += Convert.ToDouble(dgvTablesValues[4, i].Value);
                    sumFp[5] += Convert.ToDouble(dgvTablesValues[5, i].Value);

                    for (int j = 1; j < dgvTablesValues.Columns.Count; j++)
                    {
                        dgvTablesValues.Rows[10].Cells[j].Value = sumFp[j] / 10;

                    }
                }
            }
            //расчёт среднего значения Fi, x ̅_j

            //расчёт S_j^2
            for (int i = 0; i < dgvTablesValues.Rows.Count - 4; i++)
            {
                variance[1] += Math.Pow(Convert.ToDouble(dgvTablesValues[1, i].Value) - Convert.ToDouble(dgvTablesValues[1, 10].Value), 2);
                variance[2] += Math.Pow(Convert.ToDouble(dgvTablesValues[2, i].Value) - Convert.ToDouble(dgvTablesValues[1, 10].Value), 2);
                variance[3] += Math.Pow(Convert.ToDouble(dgvTablesValues[3, i].Value) - Convert.ToDouble(dgvTablesValues[1, 10].Value), 2);
                variance[4] += Math.Pow(Convert.ToDouble(dgvTablesValues[4, i].Value) - Convert.ToDouble(dgvTablesValues[1, 10].Value), 2);
                variance[5] += Math.Pow(Convert.ToDouble(dgvTablesValues[5, i].Value) - Convert.ToDouble(dgvTablesValues[1, 10].Value), 2);

                for (int j = 1; j < dgvTablesValues.Columns.Count; j++)
                {
                    dgvTablesValues[j, 11].Value = variance[j];
                }
            }
            //расчёт S_j^2

            //расчёт s_j^2
            for (int j = 1; j < dgvTablesValues.Columns.Count; j++)
            {
                dgvTablesValues[j, 12].Value = Convert.ToDouble(dgvTablesValues[j, 11].Value) / (10 - 1);
            }
            //расчёт s_j^2

            //Максимальное число в колонке
            //            double[] columnData = new double[dgvTablesValues.Rows.Count];
            //            columnData = (from DataGridViewRow row in dgvTablesValues.Rows
            //                          where
            //row.Cells[1].FormattedValue.ToString() != string.Empty
            //                          select Convert.ToDouble(row.Cells[1].FormattedValue)).ToArray();
            //Максимальное число в колонке

            //Максимальное число в строке
            double[] columnData = new double[6];
            for (int j = 1; j < dgvTablesValues.Columns.Count; j++)
            {
                columnData[j] = Convert.ToDouble(dgvTablesValues[j, 12].Value);
            }
            //dgvTablesValues[1, 13].Value = columnData.Max().ToString();
            //Максимальное число в строке

            //расчёт G_набл
            double sjSumm = 0;
            double gObserved = 0;
            for (int i = 1; i < dgvTablesValues.Columns.Count; i++)
            {
                sjSumm += Convert.ToDouble(dgvTablesValues[i, 12].Value);
                gObserved = columnData.Max() / sjSumm;
            }
            //расчёт G_набл

            lObservedG.Text = "G_набл = " + gObserved;
            lCriticalG.Text = "G_крит = " + 0.3029; //значение из таблицы Кохрена

            lСomparisonG.Visible = true;
            //сравнение G_набл и G_крит
            lСomparisonG.Text = (gObserved > 0.3029) ? "–> G_набл > G_крит, продолжать проверку гипотезы Н_0 не имеет смысла" : "–> G_набл < G_крит, можно продолжить проверку гипотезы Н_0";
            //сравнение G_набл и G_крит

            //расчёт среднего значения xj
            double xjSumm = 0, xjAverage = 0;
            for (int j = 1; j < dgvTablesValues.Columns.Count; j++)
            {
                xjSumm += Convert.ToDouble(dgvTablesValues[j, 10].Value);
                xjAverage = xjSumm / 5;
            }
            //расчёт среднего значения xj


            //расчёт S_общ^2
            double[] summGeneralS = new double[10];
            double generalS = 0;
            for (int j = 1; j < dgvTablesValues.Columns.Count; j++)
            {
                summGeneralS[0] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 0].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[1] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 1].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[2] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 2].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[3] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 3].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[4] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 4].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[5] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 5].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[6] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 6].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[7] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 7].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[8] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 8].Value) - Convert.ToDouble(xjAverage), 2);
                summGeneralS[9] += Math.Pow(Convert.ToDouble(dgvTablesValues[j, 9].Value) - Convert.ToDouble(xjAverage), 2);
            }
            for (int i = 0; i < dgvTablesValues.Rows.Count - 4; i++)
            {
                generalS += summGeneralS[i];
            }
            lGeneralS.Text = "S_общ^2 = " + generalS;
            //расчёт S_общ^2

            //расчёт x ̅
            for (int j = 1; j < dgvTablesValues.Columns.Count; j++)
            {
                dgvTablesValues[j, 13].Value = Math.Pow(Convert.ToDouble(dgvTablesValues[j, 10].Value) - Convert.ToDouble(xjAverage), 2);
            }
            //расчёт x ̅

            //расчёт суммы x ̅
            double xUnderlineSumm = 0;
            for (int j = 1; j < dgvTablesValues.Columns.Count; j++)
            {
                xUnderlineSumm += Convert.ToDouble(dgvTablesValues[j, 13].Value);
            }
            //расчёт суммы x ̅

            //расчёт S_факт^2
            lActualS.Text = "S_факт^2 = " + 10 * xUnderlineSumm;
            //расчёт S_факт^2

            //расчёт S_ост^2
            lResidualS.Text = "S_ост^2 =  " + (generalS - (10 * xUnderlineSumm));
            //расчёт S_ост^2

            //расчёт s^2_факт
            lsActual.Text = "s^2_факт = " + 10 * xUnderlineSumm / (5 - 1);
            //расчёт s^2_факт

            //расчёт s^2_окт
            lsResidual.Text = "s^2_окт = " + (generalS - (10 * xUnderlineSumm)) / (5 * (10 - 1));
            //расчёт s^2_окт

            //сравнение s^2_факт и s^2_ост
            lsСomparison.Visible = true;
            lsСomparison.Text = ((10 * xUnderlineSumm / (5 - 1)) < (generalS - (10 * xUnderlineSumm)) / (5 * (10 - 1))) ? "–> s^2_факт < s^2_ост, отсюда следует справедливость гипотезы Н_0.\nНет необходимости прибегать к критерию Фишера." : "–> s^2_факт > s^2_ост, есть основания отвергнуть гипотезу Н_0";
            //сравнение s^2_факт и s^2_ост


            lNumerator.Text = "k1 = " + (5 - 1); //Число степеней свободы числителя
            lDenominator.Text = "k1 = " + (5 * (10 - 1)); //Число степеней свободы знаменателя 

            //расчёт F_набл
            lObservedF.Text = "F_набл = " + 10 * xUnderlineSumm / (5 - 1) / ((generalS - (10 * xUnderlineSumm)) / (5 * (10 - 1)));
            //расчёт F_набл

            lCriticalF.Text = "F_крит = " + 2.58 + " - при уровне значимости α=0,05"; //значение F_крит с таблицы Фишера, при уровне значимости α=0,05

            //сравнение F_набл и F_крит
            lСomparisonF.Visible = true;
            lСomparisonF.Text = (10 * xUnderlineSumm / (5 - 1) / ((generalS - (10 * xUnderlineSumm)) / (5 * (10 - 1))) < 2.58) ? "–> F_набл < F_крит, гипотеза не противоречит статистическим данным" : "–> F_набл > F_крит, гипотеза Н_0 отвергается";
            //сравнение F_набл и F_крит
        }



        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < dgvTablesValues.Columns.Count; i++)
            {
                for (int j = 0; j < dgvTablesValues.RowCount; j++)
                {
                    dgvTablesValues.Rows[j].Cells[i].Value = null; //очистить таблицу

                }
            }

            btnResultV.Enabled = false;

            lObservedG.Text = "G_набл = ";
            lCriticalG.Text = "G_крит = ";
            lСomparisonG.Visible = false;
            lGeneralS.Text = "S_общ^2 = ";
            lActualS.Text = "S_факт^2 = ";
            lResidualS.Text = "S_ост^2 =  ";
            lsActual.Text = "s^2_факт = ";
            lsResidual.Text = "s^2_окт =  ";
            lsСomparison.Visible = false;
            lNumerator.Text = "k1 = ";
            lDenominator.Text = "k2 = ";
            lObservedF.Text = "F_набл = ";
            lCriticalF.Text = "F_крит = ";
            lСomparisonF.Visible = false;
        }


        public int indexRow, indexColumn = 0;

        //Проверка на пустоту ячеек таблицы
        private bool checkEmptyDataGridCell(int freeColumn, DataGridView dataGrid)
        {
            for (int i = 0; i < dataGrid.RowCount - 4; i++)
            {
                for (int j = 1; j < dataGrid.ColumnCount; j++)
                {
                    if ((dataGrid.Rows[i].Cells[j].Value == null) || (dataGrid.Rows[i].Cells[j].Value.ToString() == ""))
                    {
                        if (j == freeColumn)
                        {
                            j = freeColumn;
                        }
                        else
                        {
                            indexRow = i;
                            indexColumn = j;
                            return false;
                        }
                    }
                }

            }
            return true;
        }

        //Проверка на пустоту ячеек таблицы

        //Событие на изменение ячеек
        private void dgvTablesValues_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Проверка на пустоту ячеек таблицы
            if (checkEmptyDataGridCell(0, dgvTablesValues) == false)
            {
                //MessageBox.Show("Проверьте ячейки таблицы!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                btnResultV.Enabled = false;

            }
            else
            {
                btnResultV.Enabled = true;

            }
            //Проверка на пустоту ячеек таблицы

            //for (int i = 1; i < dgvTablesValues.Columns.Count; i++)
            //{
            //    //for (int j = 0; j < dgvTablesValues.RowCount - 4; j++)
            //    //{
            //        if (dgvTablesValues.Rows[0].Cells[i].Value == null || dgvTablesValues.Rows[0].Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(dgvTablesValues.Rows[0].Cells[i].Value.ToString()))
            //        {
            //            //btnResultV.Enabled = (dgvTablesValues.Rows[j].Cells[i].Value != null && dgvTablesValues.Rows[j].Cells[i].Value.ToString() != "") ? true : false;
            //            btnResultV.Enabled = false;
            //        }
            //        else
            //        {
            //            btnResultV.Enabled = true;
            //        }
            //    //}
            //}
        }


        //Событие на изменение ячеек

        //Событие
        private void pError_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Заполните все ячейки таблицы!", "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        //Событие
        //3 практическая работа




        //4 практическая работа
        private void LoadFour()
        {

            dgvObservationTable.RowCount = 12;
            //нумерация
            for (int i = 0; i < 11; ++i)
            {
                dgvObservationTable.Rows[i].Cells[0].Value = i + 1;
            }
            //нумерация


            //значения xi
            dgvObservationTable.Rows[0].Cells[1].Value = "2008";
            dgvObservationTable.Rows[1].Cells[1].Value = "2009";
            dgvObservationTable.Rows[2].Cells[1].Value = "2010";
            dgvObservationTable.Rows[3].Cells[1].Value = "2011";
            dgvObservationTable.Rows[4].Cells[1].Value = "2012";
            dgvObservationTable.Rows[5].Cells[1].Value = "2013";
            dgvObservationTable.Rows[6].Cells[1].Value = "2014";
            dgvObservationTable.Rows[7].Cells[1].Value = "2015";
            dgvObservationTable.Rows[8].Cells[1].Value = "2016";
            dgvObservationTable.Rows[9].Cells[1].Value = "2017";
            dgvObservationTable.Rows[10].Cells[1].Value = "2018";
            //значения xi

            //значения Y
            dgvObservationTable.Rows[0].Cells[2].Value = "525";
            dgvObservationTable.Rows[0].Cells[3].Value = "510";
            dgvObservationTable.Rows[0].Cells[4].Value = "493";
            dgvObservationTable.Rows[0].Cells[5].Value = "520";
            dgvObservationTable.Rows[0].Cells[6].Value = "485";

            dgvObservationTable.Rows[1].Cells[2].Value = "475";
            dgvObservationTable.Rows[1].Cells[3].Value = "536";
            dgvObservationTable.Rows[1].Cells[4].Value = "558";
            dgvObservationTable.Rows[1].Cells[5].Value = "501";
            dgvObservationTable.Rows[1].Cells[6].Value = "550";

            dgvObservationTable.Rows[2].Cells[2].Value = "530";
            dgvObservationTable.Rows[2].Cells[3].Value = "430";
            dgvObservationTable.Rows[2].Cells[4].Value = "524";
            dgvObservationTable.Rows[2].Cells[5].Value = "564";
            dgvObservationTable.Rows[2].Cells[6].Value = "534";

            dgvObservationTable.Rows[3].Cells[2].Value = "540";
            dgvObservationTable.Rows[3].Cells[3].Value = "480";
            dgvObservationTable.Rows[3].Cells[4].Value = "505";
            dgvObservationTable.Rows[3].Cells[5].Value = "465";
            dgvObservationTable.Rows[3].Cells[6].Value = "545";

            dgvObservationTable.Rows[4].Cells[2].Value = "523";
            dgvObservationTable.Rows[4].Cells[3].Value = "523";
            dgvObservationTable.Rows[4].Cells[4].Value = "560";
            dgvObservationTable.Rows[4].Cells[5].Value = "452";
            dgvObservationTable.Rows[4].Cells[6].Value = "587";


            dgvObservationTable.Rows[5].Cells[2].Value = "512";
            dgvObservationTable.Rows[5].Cells[3].Value = "528";
            dgvObservationTable.Rows[5].Cells[4].Value = "542";
            dgvObservationTable.Rows[5].Cells[5].Value = "605";
            dgvObservationTable.Rows[5].Cells[6].Value = "587";

            dgvObservationTable.Rows[6].Cells[2].Value = "536";
            dgvObservationTable.Rows[6].Cells[3].Value = "590";
            dgvObservationTable.Rows[6].Cells[4].Value = "512";
            dgvObservationTable.Rows[6].Cells[5].Value = "475";
            dgvObservationTable.Rows[6].Cells[6].Value = "546";

            dgvObservationTable.Rows[7].Cells[2].Value = "576";
            dgvObservationTable.Rows[7].Cells[3].Value = "605";
            dgvObservationTable.Rows[7].Cells[4].Value = "532";
            dgvObservationTable.Rows[7].Cells[5].Value = "524";
            dgvObservationTable.Rows[7].Cells[6].Value = "490";

            dgvObservationTable.Rows[8].Cells[2].Value = "524";
            dgvObservationTable.Rows[8].Cells[3].Value = "532";
            dgvObservationTable.Rows[8].Cells[4].Value = "560";
            dgvObservationTable.Rows[8].Cells[5].Value = "561";
            dgvObservationTable.Rows[8].Cells[6].Value = "587";

            dgvObservationTable.Rows[9].Cells[2].Value = "541";
            dgvObservationTable.Rows[9].Cells[3].Value = "561";
            dgvObservationTable.Rows[9].Cells[4].Value = "521";
            dgvObservationTable.Rows[9].Cells[5].Value = "532";
            dgvObservationTable.Rows[9].Cells[6].Value = "578";

            dgvObservationTable.Rows[10].Cells[2].Value = "515";
            dgvObservationTable.Rows[10].Cells[3].Value = "524";
            dgvObservationTable.Rows[10].Cells[4].Value = "506";
            dgvObservationTable.Rows[10].Cells[5].Value = "530";
            dgvObservationTable.Rows[10].Cells[6].Value = "501";
            //значения Y


            for (int i = 7; i < dgvObservationTable.Columns.Count; i++)
            {
                for (int j = 0; j < dgvObservationTable.RowCount; j++)
                {
                    dgvObservationTable.Rows[j].Cells[i].ReadOnly = true; //запрет на запись
                    dgvObservationTable.Rows[j].Cells[i].Style.BackColor = Color.GreenYellow;//цвет ячеек
                }
            }
            for (int j = 0; j < dgvObservationTable.Columns.Count - 7; j++)
            {
                dgvObservationTable.Rows[11].Cells[j].ReadOnly = true;//запрет на запись
                dgvObservationTable.Rows[11].Cells[j].Style.BackColor = Color.GreenYellow;//цвет ячеек
            }

            for (int j = 0; j < dgvObservationTable.Rows.Count; j++)
            {
                dgvObservationTable.Rows[j].Cells[0].ReadOnly = true;//запрет на запись
            }

        }

        private void btnResultR_Click(object sender, EventArgs e)
        {
            pGR.Visible = true;
            lСomparisonGR.Visible = true;
            double[] sumYi = new double[12];
            //расчёт среднего значения y ̅_i
            for (int i = 2; i < dgvObservationTable.Columns.Count - 7; i++)
            {
                if (dgvObservationTable.Rows[i].Cells[1].Value != null && dgvObservationTable.Rows[i].Cells[1].Value.ToString() != "")
                {
                    sumYi[0] += Convert.ToDouble(dgvObservationTable[i, 0].Value);
                    sumYi[1] += Convert.ToDouble(dgvObservationTable[i, 1].Value);
                    sumYi[2] += Convert.ToDouble(dgvObservationTable[i, 2].Value);
                    sumYi[3] += Convert.ToDouble(dgvObservationTable[i, 3].Value);
                    sumYi[4] += Convert.ToDouble(dgvObservationTable[i, 4].Value);
                    sumYi[5] += Convert.ToDouble(dgvObservationTable[i, 5].Value);
                    sumYi[6] += Convert.ToDouble(dgvObservationTable[i, 6].Value);
                    sumYi[7] += Convert.ToDouble(dgvObservationTable[i, 7].Value);
                    sumYi[8] += Convert.ToDouble(dgvObservationTable[i, 8].Value);
                    sumYi[9] += Convert.ToDouble(dgvObservationTable[i, 9].Value);
                    sumYi[10] += Convert.ToDouble(dgvObservationTable[i, 10].Value);
                    for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
                    {
                        dgvObservationTable.Rows[j].Cells[7].Value = sumYi[j] / 5;

                    }
                }
            }
            //расчёт среднего значения y ̅_i

            //расчёт S_i^2
            double[] summSi = new double[11];
            for (int j = 2; j < dgvObservationTable.Columns.Count - 7; j++)
            {
                summSi[0] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 0].Value) - Convert.ToDouble(dgvObservationTable[7, 0].Value), 2);
                summSi[1] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 1].Value) - Convert.ToDouble(dgvObservationTable[7, 1].Value), 2);
                summSi[2] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 2].Value) - Convert.ToDouble(dgvObservationTable[7, 2].Value), 2);
                summSi[3] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 3].Value) - Convert.ToDouble(dgvObservationTable[7, 3].Value), 2);
                summSi[4] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 4].Value) - Convert.ToDouble(dgvObservationTable[7, 4].Value), 2);
                summSi[5] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 5].Value) - Convert.ToDouble(dgvObservationTable[7, 5].Value), 2);
                summSi[6] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 6].Value) - Convert.ToDouble(dgvObservationTable[7, 6].Value), 2);
                summSi[7] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 7].Value) - Convert.ToDouble(dgvObservationTable[7, 7].Value), 2);
                summSi[8] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 8].Value) - Convert.ToDouble(dgvObservationTable[7, 8].Value), 2);
                summSi[9] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 9].Value) - Convert.ToDouble(dgvObservationTable[7, 9].Value), 2);
                summSi[10] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 10].Value) - Convert.ToDouble(dgvObservationTable[7, 10].Value), 2);

            }
            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                dgvObservationTable[8, i].Value = summSi[i] / (5 - 1);
            }
            //расчёт S_i^2

            //расчёт x ̅
            double summX = 0;
            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                summX += Convert.ToDouble(dgvObservationTable[1, i].Value);
            }
            dgvObservationTable[1, 11].Value = summX / 11;
            //расчёт x ̅

            //расчёт y ̅
            double summY = 0;
            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                summY += Convert.ToDouble(dgvObservationTable[7, i].Value);
            }
            dgvObservationTable[7, 11].Value = summY / 11;
            //расчёт y ̅


            pSR.Visible = true;
            lСomparisonSR.Visible = true;
            //расчёт s_в^2
            double[] summSm = new double[11];
            double summSvN = 0;
            for (int j = 2; j < dgvObservationTable.Columns.Count - 7; j++)
            {
                summSm[0] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 0].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[1] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 1].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[2] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 2].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[3] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 3].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[4] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 4].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[5] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 5].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[6] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 6].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[7] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 7].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[8] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 8].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[9] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 9].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);
                summSm[10] += Math.Pow(Convert.ToDouble(dgvObservationTable[j, 10].Value) - Convert.ToDouble(dgvObservationTable[7, 11].Value), 2);

            }
            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                summSvN += summSm[i];
                dgvObservationTable[8, 11].Value = summSvN / (11 * (5 - 1));
            }
            //расчёт s_в^2



            //Максимальное число в столбце columnData.Max()
            double[] columnData = new double[11];
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                columnData[j] = Convert.ToDouble(dgvObservationTable[8, j].Value);
            }
            //Максимальное число в столбце columnData.Max()

            //расчёт G_набл
            double summSi2 = 0;
            double gObserved = 0;
            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                summSi2 += Convert.ToDouble(dgvObservationTable[8, i].Value);
                gObserved = columnData.Max() / summSi2;
            }
            //расчёт G_набл

            lObservedR.Text = "G_набл = " + gObserved;
            lCriticalR.Text = "G_крит = " + "0.3080"; //значение из таблицы Кохрена G_кр=G(α,k,l),α=0.05;k=m-1;l=N

            lСomparisonGR.Visible = true;
            //сравнение G_набл и G_крит
            lСomparisonGR.Text = (gObserved > 0.3080) ? "–> G_набл > G_крит, условие не выполняется, надо перейти к другому варианту" : "–> G_набл < G_крит, гипотеза об однородности принимается";
            //сравнение G_набл и G_крит

            //расчёт u_i
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                dgvObservationTable[11, j].Value = Convert.ToDouble(dgvObservationTable[1, j].Value) - Convert.ToDouble(dgvObservationTable[1, 11].Value);
            }
            //расчёт u_i

            //расчёт u_i^2
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                dgvObservationTable[12, j].Value = Math.Pow(Convert.ToDouble(dgvObservationTable[11, j].Value), 2);
            }
            //расчёт u_i^2

            //расчёт u_i y ̅_i
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                dgvObservationTable[13, j].Value = Convert.ToDouble(dgvObservationTable[11, j].Value) * Convert.ToDouble(dgvObservationTable[7, j].Value);
            }
            //расчёт u_i y ̅_i


            //расчёт суммы u_i^2
            double summUi = 0;
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                summUi += Convert.ToDouble(dgvObservationTable[12, j].Value);
                dgvObservationTable[12, 11].Value = summUi;
            }
            //расчёт суммы u_i^2

            //расчёт суммы u_i y ̅_i
            double summUiYi = 0;
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                summUiYi += Convert.ToDouble(dgvObservationTable[13, j].Value);
                dgvObservationTable[13, 11].Value = summUiYi;
            }
            //расчёт суммы u_i y ̅_i

            //расчёт C1
            double c1 = 0;
            c1 = summUiYi / summUi;
            //расчёт C1

            //расчёт a
            double a = 0;
            a = Convert.ToDouble(dgvObservationTable[7, 11].Value) - c1 * Convert.ToDouble(dgvObservationTable[1, 11].Value);
            //расчёт a

            //расчёт y ̃_i
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                dgvObservationTable[9, j].Value = a + c1 * Convert.ToDouble(dgvObservationTable[1, j].Value);
            }
            //расчёт y ̃_i

            //расчёт (y ̅_i-y ̃_i )^2
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                dgvObservationTable[10, j].Value = Math.Pow(Convert.ToDouble(dgvObservationTable[7, j].Value) - Convert.ToDouble(dgvObservationTable[9, j].Value), 2);
            }
            //расчёт (y ̅_i-y ̃_i )^2

            //расчёт s_a^2
            double summYi_Yi = 0;
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                summYi_Yi += Convert.ToDouble(dgvObservationTable[10, j].Value);
                dgvObservationTable[10, 11].Value = summYi_Yi * 1 / (11 - 2);
            }
            //расчёт s_a^2

            lSa.Text = "s_a^2 = " + Convert.ToDouble(dgvObservationTable[10, 11].Value);
            lSv.Text = "s_в^2 = " + Convert.ToDouble(dgvObservationTable[8, 11].Value);

            lСomparisonSR.Visible = true;
            //сравнение s_a^2 и s_в^2
            lСomparisonSR.Text = (Convert.ToDouble(dgvObservationTable[10, 11].Value) < Convert.ToDouble(dgvObservationTable[8, 11].Value)) ? "–> s_a^2 < s_в^2, проверить гипотезу невозможно" : "–> s_a^2 > s_в^2, гипотеза Н_0 принимается";
            //сравнение s_a^2 и s_в^2

            pFR.Visible = true;

            lСomparisonFR.Visible = true;
            lNumeratorR.Text = "k1 = " + (11 - 2); //Число степеней свободы числителя
            lDenominatorR.Text = "k1 = " + (11 * (5 - 1)); //Число степеней свободы знаменателя 

            //расчёт F_набл
            lObservedFR.Text = "F_набл = " + Convert.ToDouble(dgvObservationTable[10, 11].Value) / Convert.ToDouble(dgvObservationTable[8, 11].Value);
            //расчёт F_набл

            lCriticalFR.Text = "F_крит = " + 2.05 + " - при уровне значимости α=0,05"; //значение F_крит с таблицы Фишера, при уровне значимости α=0,05

            //сравнение F_набл и F_крит
            lСomparisonFR.Text = (Convert.ToDouble(dgvObservationTable[10, 11].Value) / Convert.ToDouble(dgvObservationTable[8, 11].Value) < 2.05) ? "–> F_набл < F_крит, гипотеза об адекватности регрессионной модели принимается" : "–> F_набл > F_крит, гипотеза Н_0 отвергается.\nПостроенная регрессионная модель неадекватна";
            //сравнение F_набл и F_крит



            pTC0R.Visible = true;
            lСomparisonTC0R.Visible = true;
            lKC0R.Text = "k = " + 11 * (5 - 1);
            lAC0R.Text = "α = " + 0.05;

            //расчёт t_набл
            double observedTC0 = 0;
            observedTC0 = Math.Abs(Convert.ToDouble(dgvObservationTable[7, 11].Value)) / (Convert.ToDouble(dgvObservationTable[8, 11].Value) / Math.Sqrt(11));
            lObservedTC0R.Text = "t_набл = " + observedTC0;
            //расчёт t_набл

            double criticalTC0 = 2.015;//t_кр α=0.05; k=N(m-1) = 11
            lCriticalTC0R.Text = "t_крит = " + criticalTC0;

            //сравнение t_набл и t_крит
            lСomparisonTC0R.Text = (observedTC0 > criticalTC0) ? "–> t_набл > t_крит, гипотеза о незначимости коэффициента  C_0 отвергается, т.е. C_0≠0" : "–> t_набл < t_крит, гипотеза Н_0: C_0=0 принимается, коэффициент  C_0 незначим";
            //сравнение t_набл и t_крит



            pTC1R.Visible = true;
            lСomparisonTC1R.Visible = true;
            lKC1R.Text = "k = " + 11 * (5 - 1);
            lAC1R.Text = "α = " + 0.05;

            //расчёт t_набл
            double observedTC1 = 0;
            observedTC1 = Math.Abs(c1) / (Convert.ToDouble(dgvObservationTable[8, 11].Value) / Math.Sqrt(summUi));
            lObservedTC1R.Text = "t_набл = " + observedTC1;
            //расчёт t_набл

            double criticalTC1 = 2.015;//t_кр α=0.05; k=N(m-1) = 11
            lCriticalTC1R.Text = "t_крит = " + criticalTC1;

            //сравнение t_набл и t_крит
            lСomparisonTC1R.Text = (observedTC1 > criticalTC1) ? "–> t_набл > t_крит, гипотеза о незначимости коэффициента  C_1 отвергается, т.е. C_1≠0" : "–> t_набл < t_крит, гипотеза Н_0: C_1=0 принимается, коэффициент  C_1 незначим";
            //сравнение t_набл и t_крит


            lBuildingTrustArea.Visible = true;
            pTrustArea.Visible = true;

            //расчёт +MY(x)
            double[] myX = new double[11];

            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                myX[i] = Convert.ToDouble(dgvObservationTable[7, 11].Value) + c1 * (Convert.ToDouble(dgvObservationTable[1, i].Value) - Convert.ToDouble(dgvObservationTable[1, 11].Value)) + 2.262 * (Convert.ToDouble(dgvObservationTable[8, 11].Value) / Math.Sqrt(11)) * Math.Sqrt(1 + (Math.Pow(Convert.ToDouble(dgvObservationTable[1, i].Value) - Convert.ToDouble(dgvObservationTable[1, 11].Value), 2) / (summUi / 11)));
                lMYx.Text += "+MY(x" + (i + 1) + ") = " + myX[i] + "\n";
            }
            //расчёт +MY(x)

            //расчёт -MY(x)
            double[] myX_ = new double[11];
            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                myX_[i] = Convert.ToDouble(dgvObservationTable[7, 11].Value) + c1 * (Convert.ToDouble(dgvObservationTable[1, i].Value) - Convert.ToDouble(dgvObservationTable[1, 11].Value)) - 2.262 * (Convert.ToDouble(dgvObservationTable[8, 11].Value) / Math.Sqrt(11)) * Math.Sqrt(1 + (Math.Pow(Convert.ToDouble(dgvObservationTable[1, i].Value) - Convert.ToDouble(dgvObservationTable[1, 11].Value), 2) / (summUi / 11)));
                lMYx_.Text += "-MY(x" + (i + 1) + ") = " + myX_[i] + "\n";
            }
            //расчёт -MY(x)

            //работа с графиком
            pChart.Visible = true; //показать панель с графиком
            chartRegressionLineGraph.Titles.Clear(); //Очистить название графика
            chartRegressionLineGraph.Series[0].Points.Clear(); //Очистить точки линии 1
            chartRegressionLineGraph.Series[1].Points.Clear(); //Очистить точки линии 2
            chartRegressionLineGraph.Series[2].Points.Clear(); //Очистить точки линии 3

            chartRegressionLineGraph.ChartAreas[0].AxisX.Title = "X";//названин оси X
            chartRegressionLineGraph.ChartAreas[0].AxisY.Title = "Y";//названин оси Y

            chartRegressionLineGraph.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;// тип диаграммы
            chartRegressionLineGraph.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;// тип диаграммы
            chartRegressionLineGraph.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;// тип диаграммы

            chartRegressionLineGraph.Titles.Add("Доверительная зона для линии регрессии"); //название графика

            chartRegressionLineGraph.ChartAreas[0].AxisX.Title = "X";//названин оси X
            chartRegressionLineGraph.ChartAreas[0].AxisY.Title = "Y";//названин оси Y

            //построение 1 линии по точкам
            for (int j = 0; j < dgvObservationTable.Rows.Count - 1; j++)
            {
                chartRegressionLineGraph.Series[0].Points.AddXY(Convert.ToInt32(dgvObservationTable[1, j].Value), Convert.ToInt32(dgvObservationTable[9, j].Value));
            }
            //построение 1 линии по точкам

            //построение 2 линии по точкам
            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                chartRegressionLineGraph.Series[1].Points.AddXY(Convert.ToInt32(dgvObservationTable[1, i].Value), myX[i]);

            }
            //построение 2 линии по точкам

            //построение 3 линии по точкам
            for (int i = 0; i < dgvObservationTable.Rows.Count - 1; i++)
            {
                chartRegressionLineGraph.Series[2].Points.AddXY(Convert.ToInt32(dgvObservationTable[1, i].Value), myX_[i]);

            }
            //построение 3 линии по точкам
            //работа с графиком
        }

        public int indexRowTwo, indexColumnTwo = 0;


        //Проверка на пустоту ячеек таблицы
        private bool checkEmptyDataGridCellTwo(int freeColumn, DataGridView dataGrid)
        {
            for (int i = 0; i < dataGrid.RowCount - 1; i++)
            {
                for (int j = 1; j < dataGrid.ColumnCount - 7; j++)
                {
                    if ((dataGrid.Rows[i].Cells[j].Value == null) || (dataGrid.Rows[i].Cells[j].Value.ToString() == ""))
                    {
                        if (j == freeColumn)
                        {
                            j = freeColumn;
                        }
                        else
                        {
                            indexRowTwo = i;
                            indexColumnTwo = j;
                            return false;
                        }
                    }
                }

            }
            return true;
        }

        //Проверка на пустоту ячеек таблицы


        //Событие на изменение ячеек
        private void dgvObservationTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Проверка на пустоту ячеек таблицы
            if (checkEmptyDataGridCellTwo(10, dgvObservationTable) == false)
            {
                //MessageBox.Show("Проверьте ячейки таблицы!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                btnResultR.Enabled = false;
            }
            else
            {
                btnResultR.Enabled = true;
            }
            //Проверка на пустоту ячеек таблицы
        }


        //Событие на изменение ячеек

        //Событие
        private void pErrorR_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Заполните все ячейки таблицы!", "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



        //Событие

        //Очистить таблицу наблюдений
        private void btnClearR_Click(object sender, EventArgs e)
        {
            chartRegressionLineGraph.Series[0].Points.Clear(); //Очистить точки линии 1
            chartRegressionLineGraph.Series[1].Points.Clear(); //Очистить точки линии 2
            chartRegressionLineGraph.Series[2].Points.Clear(); //Очистить точки линии 3
            chartRegressionLineGraph.Titles.Clear(); //Очистить название графика

            pChart.Visible = false;//скрыть панель с графиком

            for (int i = 1; i < dgvObservationTable.Columns.Count; i++)
            {
                for (int j = 0; j < dgvObservationTable.RowCount; j++)
                {
                    dgvObservationTable.Rows[j].Cells[i].Value = null;
                }
            }
            pGR.Visible = false;
            lСomparisonGR.Visible = false;
            pSR.Visible = false;
            lСomparisonSR.Visible = false;
            pFR.Visible = false;
            lСomparisonFR.Visible = false;
            pTC0R.Visible = false;
            lСomparisonTC0R.Visible = false;
            pTC1R.Visible = false;
            lСomparisonTC1R.Visible = false;
            lBuildingTrustArea.Visible = false;
            pTrustArea.Visible = false;

        }
        //Очистить таблицу наблюдений

        //4 практическая работа




        //6 практическая работа
        private void LoadSix()
        {
            rbDataOne.Checked = true;
            dgvObservationTableC.RowCount = 2;

            //значения x_i
            dgvObservationTableC.Rows[0].Cells[0].ReadOnly = true; //запрет на запись
            dgvObservationTableC.Rows[0].Cells[0].Style.BackColor = Color.GreenYellow;//цвет ячейки
            dgvObservationTableC.Rows[0].Cells[0].Value = "x_i";
            dgvObservationTableC.Rows[0].Cells[1].Value = "675";
            dgvObservationTableC.Rows[0].Cells[2].Value = "645";
            dgvObservationTableC.Rows[0].Cells[3].Value = "687";
            dgvObservationTableC.Rows[0].Cells[4].Value = "608";
            dgvObservationTableC.Rows[0].Cells[5].Value = "702";
            dgvObservationTableC.Rows[0].Cells[6].Value = "682";
            dgvObservationTableC.Rows[0].Cells[7].Value = "655";
            dgvObservationTableC.Rows[0].Cells[8].Value = "691";
            dgvObservationTableC.Rows[0].Cells[9].Value = "662";
            dgvObservationTableC.Rows[0].Cells[10].Value = "671";
            dgvObservationTableC.Rows[0].Cells[11].Value = "642";
            //значения x_i


            //значения y_i
            dgvObservationTableC.Rows[1].Cells[0].ReadOnly = true; //запрет на запись
            dgvObservationTableC.Rows[1].Cells[0].Style.BackColor = Color.GreenYellow;//цвет ячейки
            dgvObservationTableC.Rows[1].Cells[0].Value = "y_i";
            dgvObservationTableC.Rows[1].Cells[1].Value = "520";
            dgvObservationTableC.Rows[1].Cells[2].Value = "429";
            dgvObservationTableC.Rows[1].Cells[3].Value = "512";
            dgvObservationTableC.Rows[1].Cells[4].Value = "475";
            dgvObservationTableC.Rows[1].Cells[5].Value = "545";
            dgvObservationTableC.Rows[1].Cells[6].Value = "587";
            dgvObservationTableC.Rows[1].Cells[7].Value = "506";
            dgvObservationTableC.Rows[1].Cells[8].Value = "511";
            dgvObservationTableC.Rows[1].Cells[9].Value = "592";
            dgvObservationTableC.Rows[1].Cells[10].Value = "553";
            dgvObservationTableC.Rows[1].Cells[11].Value = "540";

            //значения y_i
        }

        //событие, выбор загружаемых данных
        private void rbDataOne_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDataOne.Checked)
            {
                LoadSix();
            }
            else
            {
                rbData();
            }

        }


        public void rbData()
        {
            //значения x_i
            //dgvObservationTableC.Rows[0].Cells[0].ReadOnly = true; //запрет на запись
            //dgvObservationTableC.Rows[0].Cells[0].Style.BackColor = Color.GreenYellow;//цвет ячейки
            dgvObservationTableC.Rows[0].Cells[0].Value = "x_i";
            dgvObservationTableC.Rows[0].Cells[1].Value = "1010";
            dgvObservationTableC.Rows[0].Cells[2].Value = "930";
            dgvObservationTableC.Rows[0].Cells[3].Value = "1070";
            dgvObservationTableC.Rows[0].Cells[4].Value = "790";
            dgvObservationTableC.Rows[0].Cells[5].Value = "900";
            dgvObservationTableC.Rows[0].Cells[6].Value = "982";
            dgvObservationTableC.Rows[0].Cells[7].Value = "1011";
            dgvObservationTableC.Rows[0].Cells[8].Value = "934";
            dgvObservationTableC.Rows[0].Cells[9].Value = "1050";
            dgvObservationTableC.Rows[0].Cells[10].Value = "1006";
            dgvObservationTableC.Rows[0].Cells[11].Value = "986";
            //значения x_i
        }
        //событие, выбор загружаемых данных

        private void btnResultC_Click(object sender, EventArgs e)
        {
            //расчёт ρ_xy
            double summXiYi = 0;
            double summXi = 0;
            double summYi = 0;
            double summPowXi = 0, summPowYi = 0;
            double pxy = 0;
            int n = 11;
            for (int i = 1; i < dgvObservationTableC.Columns.Count; i++)
            {
                summXiYi += Convert.ToDouble(dgvObservationTableC[i, 0].Value) * Convert.ToDouble(dgvObservationTableC[i, 1].Value);
                summXi += Convert.ToDouble(dgvObservationTableC[i, 0].Value);
                summYi += Convert.ToDouble(dgvObservationTableC[i, 1].Value);
                summPowXi += Math.Pow(Convert.ToDouble(dgvObservationTableC[i, 0].Value), 2);
                summPowYi += Math.Pow(Convert.ToDouble(dgvObservationTableC[i, 1].Value), 2);

            }
            pxy = ((n * summXiYi) - summXi * summYi) / ((n * summPowXi) - Math.Pow(summXi, 2));
            lPxy.Text = "ρ_xy = " + Math.Round(pxy, 3);
            //расчёт ρ_xy

            //расчёт b
            double b = 0;
            b = (summPowXi * summYi - summXi * summXiYi) / (summPowXi - Math.Pow(summXi, 2));
            lB.Text = "b = " + Math.Round(b, 3);
            //расчёт b

            //уравнение прямой линии регрессии
            lRegressionLineEquation.Text = (b < 0) ? "Y = " + Math.Round(pxy, 3) + "x + " + "(" + Math.Round(b, 3) + ")" : "Y = " + Math.Round(pxy, 3) + "x + " + Math.Round(b, 3);
            //уравнение прямой линии регрессии

            //расчёт σ_x^2
            double σ_x = 0;
            σ_x = (summPowXi / n) - Math.Pow(summXi / n, 2);
            lσ_x.Text = "σ_x^2 = " + Math.Round(σ_x, 3);
            //расчёт σ_x^2

            //расчёт σ_y^2
            double σ_y = 0;
            σ_y = (summPowYi / n) - Math.Pow(summYi / n, 2);
            lσ_y.Text = "σ_y^2 = " + Math.Round(σ_y, 3);
            //расчёт σ_y^2

            //расчёт r_в
            double rv = 0;
            rv = pxy * (Math.Sqrt(σ_x) / Math.Sqrt(σ_y));
            lCorrelationCoefficient.Text = "r_в = " + Math.Round(rv, 3);
            //расчёт r_в

            //расчёт Т_набл
            double observedT = 0;
            observedT = rv * Math.Sqrt((n - 2) / (1 - rv));
            lObservedTC.Text = "Т_набл = " + Math.Abs(Math.Round(observedT, 3));
            //расчёт Т_набл

            lKC.Text = "k = " + (n - 2); // Число степеней свободы 
            lAC.Text = "α = " + 0.05; // уровень значимости 
            double criticalT = 2.3060;// t_кр, из таблицы Стьюдента
            lCriticalC.Text = "t_кр = " + criticalT;

            lСomparisonC.Visible = true;
            pRegressionLineEquation.Visible = true;
            pSampleCorrelationCoefficient.Visible = true;
            pSignificanceSampleCorrelationCoefficient.Visible = true;
            lСomparisonC.Text = (Math.Abs(observedT) < criticalT) ? "|T_набл| < t_кр, выборочный  коэффициент корреляции незначим, нет оснований считать случайные величины коррелированными" : "|T_набл| > t_кр, выборочный коэффициент корреляции значимо отличается от нуля случайные величины Y на Х коррелированы";


            //работа с графиком
            double[] observationalDataFormula = new double[12];

            //for (int i = 1; i < dgvObservationTableC.Rows.Count; i++)
            //{
            //    //observationalDataFormula[i] = pxy * Convert.ToDouble(dgvObservationTableC[i, 0].Value) + b;

            //}
            
            pChartCorrelationAnalysis.Visible = true; //показать панель с графиком
            chartCorrelationAnalysis.Titles.Clear(); //Очистить название графика
            chartCorrelationAnalysis.Series[0].Points.Clear(); //Очистить точки линии 1
            chartCorrelationAnalysis.Series[1].Points.Clear(); //Очистить точки линии 2

            chartCorrelationAnalysis.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;// тип диаграммы
            chartCorrelationAnalysis.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;// тип диаграммы

            chartCorrelationAnalysis.Titles.Add("Уравнение регрессии"); //название графика

            chartCorrelationAnalysis.ChartAreas[0].AxisX.Title = "X";//названин оси X
            chartCorrelationAnalysis.ChartAreas[0].AxisY.Title = "Y";//названин оси Y

            //построение 1 линии по точкам
            for (int j = 1; j < dgvObservationTableC.Columns.Count; j++)
            {
                chartCorrelationAnalysis.Series[0].Points.AddXY(Convert.ToInt32(dgvObservationTableC[j, 0].Value), Convert.ToInt32(dgvObservationTableC[j, 1].Value));
            }
            //построение 1 линии по точкам

            //добавление элементов в массив
            for (int i = 1; i < dgvObservationTableC.Columns.Count; i++)
            {
                observationalDataFormula[i] = Convert.ToDouble(dgvObservationTableC[i, 0].Value);
            }
            //добавление элементов в массив

            double[] observationalDataFormulaMin = new double[12];
            observationalDataFormulaMin = observationalDataFormula.Where(x => x != 0).ToArray(); //убрать 0 из массива
          
            double max = observationalDataFormula.Max(); //максимальное число из массива 
            double min = observationalDataFormulaMin.Min(); //минимальное число из массива 

            //выбор индекса 
            int dgvIndexYmin = 0;
            for (int i = 1; i < dgvObservationTableC.Columns.Count; i++)
            {
                if (Convert.ToDouble(dgvObservationTableC[i, 0].Value) == min)
                {
                    dgvIndexYmin = i;
                }
            }
            int dgvIndexYmax = 0;
            for (int i = 1; i < dgvObservationTableC.Columns.Count; i++)
            {
                if (Convert.ToDouble(dgvObservationTableC[i, 0].Value) == max)
                {
                    dgvIndexYmax = i;
                }
            }
            //выбор индекса 

            //построение 2 линии по точкам
            chartCorrelationAnalysis.Series[1].Points.AddXY(min, dgvObservationTableC[dgvIndexYmin, 1].Value);
            chartCorrelationAnalysis.Series[1].Points.AddXY(max, dgvObservationTableC[dgvIndexYmax, 1].Value);
            //построение 2 линии по точкам

            //работа с графиком
        }

        //Очистить таблицу наблюдений
        private void btnClearC_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < dgvObservationTableC.Columns.Count; i++)
            {
                for (int j = 0; j < dgvObservationTableC.RowCount; j++)
                {
                    dgvObservationTableC[i, j].Value = null;
                }
            }

            lСomparisonC.Visible = false;
            pRegressionLineEquation.Visible = false;
            pSampleCorrelationCoefficient.Visible = false;
            pSignificanceSampleCorrelationCoefficient.Visible = false;
            pChartCorrelationAnalysis.Visible = false;
        }
        //Очистить таблицу наблюдений

        //Событие
        private void pErrorC_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Заполните все ячейки таблицы!", "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        //Событие

        public int indexRowTwoC, indexColumnTwoC = 0;
        //Проверка на пустоту ячеек таблицы
        private bool checkEmptyDataGridCellTwoC(int freeColumn, DataGridView dataGrid)
        {
            for (int i = 0; i < dataGrid.RowCount; i++)
            {
                for (int j = 1; j < dataGrid.ColumnCount; j++)
                {
                    if ((dataGrid.Rows[i].Cells[j].Value == null) || (dataGrid.Rows[i].Cells[j].Value.ToString() == ""))
                    {
                        if (j == freeColumn)
                        {
                            j = freeColumn;
                        }
                        else
                        {
                            indexRowTwoC = i;
                            indexColumnTwoC = j;
                            return false;
                        }
                    }
                }

            }
            return true;
        }
        //Проверка на пустоту ячеек таблицы


        //Событие на изменение ячеек
        private void dgvObservationTableC_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Проверка на пустоту ячеек таблицы
            if (checkEmptyDataGridCellTwoC(0, dgvObservationTableC) == false)
            {
                btnResultC.Enabled = false;
            }
            else
            {
                btnResultC.Enabled = true;
            }
            //Проверка на пустоту ячеек таблицы
        }
        //Событие на изменение ячеек
        //6 практическая работа




        //8 практическая работа
        private void LoadEight()
        {

            dgvDelayMatrix.RowCount = 4;
            //нумерация
            for (int i = 0; i < 4; ++i)
            {
                dgvDelayMatrix.Rows[i].Cells[0].Value = i + 1;
            }
            //нумерация

            //значения x (матрица издержек)
            dgvDelayMatrix.Rows[0].Cells[1].Value = "20";
            dgvDelayMatrix.Rows[0].Cells[2].Value = "22";
            dgvDelayMatrix.Rows[0].Cells[3].Value = "19";
            dgvDelayMatrix.Rows[0].Cells[4].Value = "18";
            dgvDelayMatrix.Rows[0].Cells[5].Value = "19";
            dgvDelayMatrix.Rows[0].Cells[6].Value = "19";
            dgvDelayMatrix.Rows[0].Cells[7].Value = "18";

            dgvDelayMatrix.Rows[1].Cells[1].Value = "19";
            dgvDelayMatrix.Rows[1].Cells[2].Value = "20";
            dgvDelayMatrix.Rows[1].Cells[3].Value = "22";
            dgvDelayMatrix.Rows[1].Cells[4].Value = "20";
            dgvDelayMatrix.Rows[1].Cells[5].Value = "19";
            dgvDelayMatrix.Rows[1].Cells[6].Value = "18";
            dgvDelayMatrix.Rows[1].Cells[7].Value = "19";

            dgvDelayMatrix.Rows[2].Cells[1].Value = "19";
            dgvDelayMatrix.Rows[2].Cells[2].Value = "20";
            dgvDelayMatrix.Rows[2].Cells[3].Value = "21";
            dgvDelayMatrix.Rows[2].Cells[4].Value = "20";
            dgvDelayMatrix.Rows[2].Cells[5].Value = "21";
            dgvDelayMatrix.Rows[2].Cells[6].Value = "22";
            dgvDelayMatrix.Rows[2].Cells[7].Value = "21";

            dgvDelayMatrix.Rows[3].Cells[1].Value = "21";
            dgvDelayMatrix.Rows[3].Cells[2].Value = "22";
            dgvDelayMatrix.Rows[3].Cells[3].Value = "21";
            dgvDelayMatrix.Rows[3].Cells[4].Value = "21";
            dgvDelayMatrix.Rows[3].Cells[5].Value = "23";
            dgvDelayMatrix.Rows[3].Cells[6].Value = "24";
            dgvDelayMatrix.Rows[3].Cells[7].Value = "22";

            //значения x (матрица издержек)

            for (int j = 0; j < dgvDelayMatrix.Rows.Count; j++)
            {
                dgvDelayMatrix.Rows[j].Cells[0].ReadOnly = true;//запрет на запись
            }

        }

        //Очистить матрицу издержек
        private void btnClearM_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < dgvDelayMatrix.Columns.Count; i++)
            {
                for (int j = 0; j < dgvDelayMatrix.RowCount; j++)
                {
                    dgvDelayMatrix[i, j].Value = null;
                }
            }

            pCoefficients.Visible = false;
            lValueXm.Visible = false;
        }
        //Очистить матрицу издержек


        //Событие
        private void pErrorM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Заполните все ячейки матрицы!", "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        //Событие

        public int indexRowTwoM, indexColumnTwoM = 0;
        //Проверка на пустоту ячеек таблицы
        private bool checkEmptyDataGridCellTwoM(int freeColumn, DataGridView dataGrid)
        {
            for (int i = 0; i < dataGrid.RowCount; i++)
            {
                for (int j = 1; j < dataGrid.ColumnCount; j++)
                {
                    if ((dataGrid.Rows[i].Cells[j].Value == null) || (dataGrid.Rows[i].Cells[j].Value.ToString() == ""))
                    {
                        if (j == freeColumn)
                        {
                            j = freeColumn;
                        }
                        else
                        {
                            indexRowTwoM = i;
                            indexColumnTwoM = j;
                            return false;
                        }
                    }
                }

            }
            return true;
        }
        //Проверка на пустоту ячеек таблицы


        //Событие на изменение ячеек
        private void dgvDelayMatrix_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Проверка на пустоту ячеек таблицы
            if (checkEmptyDataGridCellTwoM(0, dgvDelayMatrix) == false)
            {
                btnResultM.Enabled = false;
            }
            else
            {
                btnResultM.Enabled = true;
            }
            //Проверка на пустоту ячеек таблицы
        }
        //Событие на изменение ячеек

        //Рассчитать
        private void btnResultM_Click(object sender, EventArgs e)
        {
            //расчёт A0
            int n = dgvDelayMatrix.Columns.Count - 1;
            double summA0A1 = 0;
            double summA0 = 0;
            double summA1 = 0;
            double summPowA0 = 0;
            double a0 = 0;
            for (int i = 1; i < dgvDelayMatrix.Columns.Count; i++)
            {
                summA0A1 += Convert.ToDouble(dgvDelayMatrix[i, 0].Value) * Convert.ToDouble(dgvDelayMatrix[i, 1].Value);
                summA0 += Convert.ToDouble(dgvDelayMatrix[i, 0].Value);
                summA1 += Convert.ToDouble(dgvDelayMatrix[i, 1].Value);
                summPowA0 += Math.Pow(Convert.ToDouble(dgvDelayMatrix[i, 0].Value), 2);

            }
            a0 = ((n * summA0A1) - summA0 * summA1) / ((n * summPowA0) - Math.Pow(summA0, 2));
            lRatioA0.Text = "A0 = " + Math.Round(a0, 3);
            //расчёт A0

            //расчёт A1
            double a1 = 0;
            a1 = (summPowA0 * summA1 - summA0 * summA0A1) / (summPowA0 - Math.Pow(summA0, 2));
            //a1 = (summYi - a0 * summXi) / n;
            lRatioA1.Text = "A1 = " + Math.Round(a1, 3);
            //расчёт A1

            //расчёт A2
            double summA2A3 = 0;
            double summA2 = 0;
            double summA3 = 0;
            double summPowA2 = 0;
            double a2 = 0;
            for (int i = 1; i < dgvDelayMatrix.Columns.Count; i++)
            {
                summA2A3 += Convert.ToDouble(dgvDelayMatrix[i, 2].Value) * Convert.ToDouble(dgvDelayMatrix[i, 3].Value);
                summA2 += Convert.ToDouble(dgvDelayMatrix[i, 2].Value);
                summA3 += Convert.ToDouble(dgvDelayMatrix[i, 3].Value);
                summPowA2 += Math.Pow(Convert.ToDouble(dgvDelayMatrix[i, 2].Value), 2);

            }
            a2 = ((n * summA2A3) - summA2 * summA3) / ((n * summPowA2) - Math.Pow(summA2, 2));
            lRatioA2.Text = "A2 = " + Math.Round(a2, 3);
            //расчёт A2

            //расчёт A3
            double a3 = 0;
            a3 = (summPowA2 * summA3 - summA2 * summA2A3) / (summPowA2 - Math.Pow(summA2, 2));
            //a1 = (summYi - a0 * summXi) / n;
            lRatioA3.Text = "A3 = " + Math.Round(a3, 3);
            //расчёт A3

            pCoefficients.Visible = true;

            double ValueXm = 0;

            ValueXm = Math.Abs(a3) + Math.Abs(a2) * Convert.ToDouble(dgvDelayMatrix[7, 0].Value) + Math.Abs(a2) + Math.Abs(a1) * Convert.ToDouble(dgvDelayMatrix[7, 1].Value) + Math.Abs(a1) + Math.Abs(a0) * Convert.ToDouble(dgvDelayMatrix[7, 2].Value) + Math.Abs(a1) + Math.Abs(a0) * Convert.ToDouble(dgvDelayMatrix[7, 3].Value);

            lValueXm.Text = "Xm+1 = " + Math.Round(ValueXm,3) + " °C, температура следующей ночью";
            lValueXm.Visible = true;

        }
        //Рассчитать

        //8 практическая работа
    }

}

