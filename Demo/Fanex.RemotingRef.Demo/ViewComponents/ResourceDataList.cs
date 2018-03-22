namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Microsoft.AspNetCore.Mvc;

    public class ResourceDataList : ViewComponent
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public ResourceDataList(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var inputData = new[]
            {
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "1" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "11" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "12" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "13" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "2" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "3" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "4" },
            };

            var viewModels = new List<ResourceDataViewModel>();

            foreach (var input in inputData)
            {
                var viewModel = new ResourceDataViewModel
                {
                    DataType = "Ref",
                    TypeId = input.RefId,
                    ResourceId = input.ResourceId,
                    RefDataResult = _remotingRefDataAccess.GetResourceData(input.RefId, input.ResourceId)
                };

                viewModels.Add(viewModel);
            }

            return View("~/Views/Ref/ResourceData/ResourceDataList.cshtml", viewModels);
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}