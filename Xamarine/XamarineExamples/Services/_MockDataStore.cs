using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;

[assembly: Xamarin.Forms.Dependency(typeof(ProjectInsightMobile.Services.MockDataStore))]
namespace ProjectInsightMobile.Services
{
    public class MockDataStore : IDataStore<MyWorkItem>
    {
        List<MyWorkItem> items;

        public MockDataStore()
        {
            items = new List<MyWorkItem>();
            var mockItems = new List<MyWorkItem>
            {
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_check.png", Title = "Very Cool Task", Description="This is an item description." },
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_project.png", Title = "Awesome Project", Description="This is an item description." },
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_project.png", Title = "Very Epic Project", Description="This is an item description." },
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_task.png", Title = "Lots To Do", Description="This is an item description." },
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_issue.png", Title = "Brand New Issue", Description="This is an item description." },
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_issue.png", Title = "Super Big Issue", Description="This is an item description." },
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_issue.png", Title = "Design Issue", Description="This is an item description." },
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_task.png", Title = "Demo item", Description="This is an item description." },
                //new MyWorkItem { Id = Guid.NewGuid(), Icon = "item_project.png", Title = "Demo item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(MyWorkItem item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(MyWorkItem item)
        {
            var _item = items.Where((MyWorkItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(MyWorkItem item)
        {
            var _item = items.Where((MyWorkItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<MyWorkItem> GetItemAsync(Guid id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<MyWorkItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}