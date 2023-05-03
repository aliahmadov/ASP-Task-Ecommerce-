using App.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Abstract
{
    public interface IProductService
    {
        List<Product> GetAll();
        List<Product> GetAllByCategory(int categoryId);
        List<Product> GetProductsAZ(List<Product> list,bool state);
        List<Product> GetProductsByFilterLoworHigh(List<Product> list,bool state);
        void Add(Product product);  
        void Update(Product product);
        void Delete(int id);
        Product GetById(int id);

    }
}
