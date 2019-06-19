using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.GenericRepository
{
    //public class AuthorRepository : IRepository<Author>
    //{
    //    Model1 _authorContext;

    //    public AuthorRepository()
    //    {
    //        _authorContext = new Model1();
    //    }

    //    public IEnumerable<Author> List
    //    {
    //        get
    //        {
    //            return _authorContext.Authors;
    //        }
    //    }

    //    public void Add(Author entity)
    //    {
    //        _authorContext.Authors.Add(entity);
    //        _authorContext.SaveChanges();
    //    }

    //    public void Delete(Author entity)
    //    {
    //        _authorContext.Authors.Remove(entity);
    //        _authorContext.SaveChanges();
    //    }

    //    public void Update(Author entity)
    //    {
    //        _authorContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
    //        _authorContext.SaveChanges();
    //    }

    //    public Author FindById(int Id)
    //    {
    //        var result = (from r in _authorContext.Authors where r.Id == Id select r).FirstOrDefault();
    //        return result;
    //    }

    //}

    //internal class Model1
    //{
    //    public void Entry()
    //    {
    //        throw new NotImplementedException();
    //    }

       
    //}
}
