using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using InvCtrl.Concrete;
using InvCtrl.Entities;


namespace InvCtrl
{
    public partial class Form1 : Form
    {

        EFDbContext context;         
        Cart cart;
        Sale sale;

        public Form1()
        {
            InitializeComponent(); //
            context = new EFDbContext();
            cart = new Cart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IEnumerable<Article> articles = context.Articles;
            dataGridView1.DataSource = articles.ToList();  // Список товаров в наличие
        }

        // ============== Добавить в Cart со склада =================================
        private void btAdd_Click(object sender, EventArgs e)
        {
            // Выбранный товар по ArticleId
            int articleId = 0;
            bool converted = Int32.TryParse( dataGridView1.Rows[ dataGridView1.SelectedCells[0].RowIndex].Cells["ArticleId"].Value.ToString(), out articleId);
            if (converted == false) return;
            
            // ---------  Получаем позицию склада -----------
            Article article = context.Articles.Find(articleId);

            // ------- если выбранное кол-во меньше чем на складе 
            if (article.Cnt >= (int)numUpDown.Value)
            {
                // ------ добавляем выбранное кол-во
                cart.AddItem(article, (int)numUpDown.Value);
                // ------ уменьшаем кол-во на складе
                article.Cnt = article.Cnt - (int)numUpDown.Value;
                context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Недостаточное количество на складе");
            }

            numUpDown.Value = 1; // сброс кол-ва в 1 

            // ----- Переформируем список -------
            listCart.Items.Clear();
            foreach (var line in cart.Lines)
            {
                listCart.Items.Add(String.Format("{0}   {1}   {2}", line.Article.Name, line.Quantity, line.Article.Price * line.Quantity ));
            }           
            // ----- Обновление на экране -------
            RefreshControl(sender, null);
        }

        // =============== Завершить операцию ====================
        private void btnCalcAmount_Click(object sender, EventArgs e)
        {
            // ---- Подсчёт остатка
            decimal remAmount = inputCashAmount.Value - cart.ComputeTotalValue(); 
            labelRemAmount.Text = remAmount.ToString();

            // ------ Закрытие операции и сохранение -----------
            sale.Status = "Закрыто";
            sale.DateStatus = DateTime.Now;
            sale.Amount = cart.ComputeTotalValue();
            context.SaveChanges();

            RefreshControl(sender, sale); // обновить на экране

        }

        // ========== Проверка кол-ва полученных средств ====================
        private void inputCashAmount_ValueChanged(object sender, EventArgs e)
        {
            if (inputCashAmount.Value >= cart.ComputeTotalValue() && listCart.Items.Count > 0 )
            {
                btnCalcAmount.Enabled = true;
            }
            else
            {
                btnCalcAmount.Enabled = false;
            }
        }

        // ======== Начать =======================
        private void btStart_Click(object sender, EventArgs e)
        {
            //--------------- Очищаем список товаров -----------------
            cart.Clear();
            listCart.Items.Clear();
            //--------------- Добавляем инф. об операции -------------
            sale = context.Sales.Add(new Sale { Type = "Продажа", Status = "Открыто", DateStatus = DateTime.Now });
            context.SaveChanges();           
            RefreshControl(sender, sale); // Обновить на экране

        }

        //======== Обновить инф. на экране ==================
        private void RefreshControl(object sender, Sale sale)
        {
            //------ Инф. по документу продаж
            if (sale != null) 
            {
                labelSaleId.Text = sale.SaleId.ToString();
                labelDoc.Text = String.Format("{0} ({1})", sale.Type, sale.Status);
            }

            // ------ Начать
           if ( (sender as Button).Name == "btStart" )
           {  
               labelSum.Text = "0";
               labelRemAmount.Text = "0";
               labelCntPos.Text = "0";
               inputCashAmount.Value = 0;

               btStart.Enabled = false;  // Начать недоступна
               btAdd.Enabled = true;  // Добавмть
               
           }  // ----- Завершить операцию
           else if ((sender as Button).Name == "btnCalcAmount")  
           {
               btnCalcAmount.Enabled = false; // --- Завершить операцию
               btStart.Enabled = true; // ---- Начать
               btAdd.Enabled = false; // -- Добавить
           }  // ------- Добавить
           else if ((sender as Button).Name == "btAdd")
           {
               labelSum.Text = cart.ComputeTotalValue().ToString(); // Отображаем Сумму Чека
               labelCntPos.Text = cart.Lines.Count().ToString();  // всего позиций
               dataGridView1.Refresh();
           }



        }



    }
}
