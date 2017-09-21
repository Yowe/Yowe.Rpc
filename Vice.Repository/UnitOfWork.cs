using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vice.Entity;

namespace Vice.Repository
{
    public class UnitOfWork :IUnitOfWork, IDisposable
    {
        private object obj = new object();
        private ViceDataContext context = new ViceDataContext();
        private Repository<Base_User> userRepository;
        private Repository<Base_Article> articleRepository;
        private Repository<Base_SuperStar> starRepository;
        private Repository<Base_Category> categoryRepository;
        private Repository<Base_ImageLinks> imagesRepository;
        private Repository<Base_Notify> notifyRepository;
        private Repository<Base_Schedule> scheduleRepository;
        private Repository<Base_ScrollImage> scrollRepository;
        private bool disposed = false;

        public Repository<Base_User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                    this.userRepository = new Repository<Base_User>(context);
                return this.userRepository;
            }
        }
        public Repository<Base_Article> ArticleRepository
        {
            get
            {
                if (this.articleRepository == null)
                {
                    articleRepository = new Repository<Base_Article>(context);
                }
                return articleRepository;
            }
        }
        public Repository<Base_SuperStar> StarRepository
        {
            get
            {
                if (this.starRepository == null)
                {
                    this.starRepository = new Repository<Base_SuperStar>(context);
                }
                return this.starRepository;
            }
        }
        public Repository<Base_ImageLinks> ImageRepository
        {
            get
            {
                if (this.imagesRepository == null)
                {
                    this.imagesRepository = new Repository<Base_ImageLinks>(context);
                }
                return this.imagesRepository;
            }
        }
        public Repository<Base_Notify> NotifyRepository
        {
            get
            {
                if (this.notifyRepository == null)
                    this.notifyRepository = new Repository<Base_Notify>(context);
                return this.notifyRepository;
            }
        }
        public Repository<Base_Schedule> ScheduleRepository
        {
            get
            {
                if (this.scheduleRepository == null)
                {
                    this.scheduleRepository = new Repository<Base_Schedule>(context);
                }
                return this.scheduleRepository;
            }
        }
        public Repository<Base_ScrollImage> ScrollRepository
        {
            get
            {
                if (this.scrollRepository == null)
                {
                    this.scrollRepository = new Repository<Base_ScrollImage>(context);
                }
                return this.scrollRepository;
            }
        }
        public Repository<Base_Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new Repository<Base_Category>(context);
                }
                return this.categoryRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
