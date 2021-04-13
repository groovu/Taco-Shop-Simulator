using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TSSWpf.UserWindow;

namespace TSSWpf
{
    class Shop
    {
        public User Owner { get; set; }
        public string ShopName { get; set; }
        public Dictionary<ingredients, int> stock = new Dictionary<ingredients, int>();
        //public List<Employee> employees = new List<Employee>();
        public int employees;
        public List<recipes> recipes = new List<recipes>();
        public List<string> ingrList;
        
        public Shop()
        {

        }
        public Dictionary<string, int> StockPrint()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach(var i in stock)
            {
                dict.Add(i.Key.ingredient, i.Value);
            }
            return dict;
        }
        public void ingredientList(Dictionary<ingredients, int> stock)
        {
            List<string> ingrlist = new List<string>();
            foreach(var i in stock)
            {
                ingrlist.Add(i.Key.ingredient);
            }
            ingrList = ingrlist;
        }

        internal void updateStock(List<buyItem> list, TacoDBEntity db)
        {
            //this is the ugliest thing I've ever written.
            //lesson: plan out your objects data structures
            //well in advance, so you don't have to do
            //bootleg things like this.
            //Dictionary<string, int> dict = new Dictionary<string, int>();
            //foreach (buyItem b in list)
            //{
            //    dict.Add(b.Ingredient, b.Qty);
            //}
            //Dictionary<ingredients, int> newStock = new Dictionary<ingredients, int>();
            //List<ingredients> removeStock = new List<ingredients>();
            //foreach (var i in stock)
            //{
            //    string ingr = i.Key.ingredient;
            //    if (dict.ContainsKey(ingr))
            //    {
            //        removeStock.Add(i.Key);
            //        newStock.Add(i.Key, i.Value + dict[ingr]);
            //    }
            //}
            //foreach (var i in removeStock)
            //{
            //    stock.Remove(i);
            //}
            //foreach (var i in newStock)
            //{
            //    stock.Add(i.Key, i.Value);
            //}

            foreach (buyItem b in list)
            {
                string name = b.Ingredient;
                var ingObj = getIngredient(name, db);
                if (stock.ContainsKey(ingObj))
                {
                    int qty = stock[ingObj] + b.Qty;
                    stock.Remove(ingObj);
                    stock.Add(ingObj, qty);
                } else
                {
                    stock.Add(ingObj, b.Qty);
                }
            }
        }

        public ingredients getIngredient(string ingredient, TacoDBEntity db)
        {
            var q = (from u in db.ingredients where u.ingredient == ingredient select u).FirstOrDefault();
            if (q == null)
            {
                throw new Exception("Ingredient not found.");
            }
            else
            {
                return (ingredients)q;
            }
        }
    }
}
