using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vice.Entity;
using Vice.Repository;
using System.Threading.Tasks;

namespace ViceTest
{
    [TestClass]
    public class ViceTest
    {
        [TestMethod]
        public void TestInsertParell()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            unitOfWork.NotifyRepository.Insert(new Base_Notify()
            {
                CreateDate = DateTime.Now,
                Notify_Content = "1",
                UserId = "1",
                Notify_Id=new Guid().ToString()
            });
            unitOfWork.Save();
        }
    }
}
