using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket(10);
            supermarket.Work();
        }
    }

    class Supermarket
    {
        private Queue<Bayer> _bayers = new Queue<Bayer>();
        private int _money = 0;

        public Supermarket(int bayersCount)
        {
            for (int i = 0; i < bayersCount; i++)
            {
                _bayers.Enqueue(new Bayer());
            }
        }

        public void Work()
        {
            while (_bayers.Count > 0)
            {
                ShowMoney();
                Bayer bayer = _bayers.Dequeue();
                bayer.ShowInfo();
                _money += bayer.CheckSolvensy();
                Console.ReadKey(true);
            }
        }

        private void ShowMoney()
        {
            Console.Clear();
            Console.WriteLine("На счету магазина: " + _money + " рублей.\n");
        }
    }

    class Bayer
    {
        private static Random _random = new Random();
        private Basket _basket;
        private int _money;
        private int _minMoney = 1000;
        private int _maxMoney = 2501;
        private int _minProductsCount = 10;
        private int _maxProductsCount = 20;

        public Bayer()
        {
            _basket = new Basket(_minProductsCount, _maxProductsCount);
            _money = _random.Next(_minMoney, _maxMoney);
        }

        public int CheckSolvensy()
        {
            Console.WriteLine($"Сумма покупки составляет {_basket.GetSumPrice()}. У покупателя сейчас: {_money} рублей.");

            if (_money <= _basket.GetSumPrice())
            {               
                RemoveProduct();               
            }

            Console.WriteLine($"\nПокупка прошла успешно. Цена товаров: {_basket.GetSumPrice()} рублей.");
            return _basket.GetSumPrice();
        }        

        public void ShowInfo()
        {
            Console.WriteLine("К кассе подошел покупатель. \nВ корзине у него:");
            _basket.ShowInfo();
        }

        private void RemoveProduct()
        {
            Console.WriteLine("\nУ покупателя недостаточно денег\n");

            while (_money < _basket.GetSumPrice())
            {
                _basket.RemoveProduct();
            }            
        }
    }

    class Basket
    {
        private static Random _random = new Random();
        private List<Product> _products = new List<Product>();
        private Range _range = new Range();

        public Basket(int minCount, int maxCount)
        {
            GetNewProducts(minCount, maxCount);
            SortByPrise();
        }

        public int GetSumPrice()
        {
            int sumPrice = 0;

            foreach (Product product in _products)
            {
                sumPrice += product.Price;
            }

            return sumPrice;
        }

        public void RemoveProduct()
        {
            Product product = _products[_random.Next(_products.Count)];
            _products.Remove(product);
            Console.WriteLine($"Из корзины убрали {product.Name} по цене: {product.Price} рублей.\nОбщая цена всех покупок сейчас составляет: {GetSumPrice()} рублей.");
        }

        public void ShowInfo()
        {
            foreach (Product product in _products)
            {
                Console.WriteLine($"{product.Name}. Цена: {product.Price} рублей.");
            }
        }

        private void SortByPrise()
        {
            List<Product> sortProducts = new List<Product>(0);
            List<int> prices = new List<int>();

            foreach (Product product in _products)
            {
                prices.Add(product.Price);
            }

            int previousPrise = 0;

            prices.Sort();

            foreach (int price in prices)
            {
                if (previousPrise != price)
                {
                    for (int i = 0; i < _products.Count; i++)
                    {
                        if (price == _products[i].Price)
                        {
                            sortProducts.Add(_products[i]);
                        }
                    }
                }

                previousPrise = price;
            }

            _products = sortProducts;
        }

        private void GetNewProducts(int minCount, int maxCount)
        {
            for (int i = 0; i <= _random.Next(minCount, maxCount); i++)
            {
                _products.Add(_range.GetProduct(_random.Next(_range.GetCount())));
            }
        }
    }

    class Range
    {
        private List<Product> _products = new List<Product>();

        public Range()
        {
            _products.Add(new Product("Колбаса", 200));
            _products.Add(new Product("Сыр", 150));
            _products.Add(new Product("Горошек в банке", 80));
            _products.Add(new Product("Водка", 250));
            _products.Add(new Product("Соленые огурцы", 125));
            _products.Add(new Product("Сок яблочный", 130));
            _products.Add(new Product("Яйца - 10 шт", 100));
            _products.Add(new Product("Хлеб", 25));
            _products.Add(new Product("Марочное Вино", 300));
            _products.Add(new Product("Картофель 1 кг", 65));
            _products.Add(new Product("Шоколад", 90));
            _products.Add(new Product("Мороженое", 80));
        }

        public int GetCount()
        {
            return _products.Count;
        }

        public Product GetProduct(int index)
        {
            return _products[index];
        }
    }

    class Product
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}
