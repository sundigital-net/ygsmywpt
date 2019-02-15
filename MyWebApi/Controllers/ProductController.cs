using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebApi.DTO;
using MyWebApi.IService;
using MyWebApi.Model;
using MyWebApi.Service;

namespace MyWebApi.Controllers
{
    //Routing路由
    //路由有两种方式， Convention-based (按约定), Attribute-based(基于路由属性配置的). 
    //其中Convention-based主要用于MVC(返回View或者razor page的)
    //Web Api推荐使用Attribute-based

    //使用[Route("api/[controller]")], 它使得整个Controller下面所有action的uri前缀变成了"/api/product", 
    //其中[controller]表示XxxController.cs中的Xxx(其实是小写).
    //也可以具体指定，如下，这样的好处是，以后ProductController后期重构改名了，只要不改Route的内容，请求的地址就不会变
    [Route("product")]
    public class ProductController:Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMailService _mailService;
        private readonly IProductService _productSvc;
        public ProductController(ILogger<ProductController> logger,IMailService mailService,IProductService productSvc)
        {
            _logger = logger;
            _mailService = mailService;
            _productSvc = productSvc;
        }

        //然后在List  action上面, 写上HttpGet, 也可以写HttpGet(). 它里面还可以加参数,例如: HttpGet("list"), 
        //那么这个Action的请求的地址就变成了 "/api/product/list".
        [HttpGet]
        [Route("list")]
        /*
        public JsonResult List()
        {
            return new JsonResult(ProductService.Current.Products);
        }
        */

        public IActionResult List()
        {
            var products = _productSvc.GetAll();
            return Ok(products);
        }
        
        //这里Route参数里面的{id}表示该action有一个参数名字是id. 这个action的地址是: "/product/index/{id}"
        [HttpGet]
        [Route("index/{id}",Name ="Index")]
        /*
        public JsonResult Index(long id)
        {
            return new JsonResult(ProductService.Current.Products.SingleOrDefault(t => t.Id == id));
        }
        */
        public IActionResult Index(long id)
        {
            try
            {
                //throw new ArgumentException("主动抛个异常！！！");
                var product = _productSvc.GetById(id);
                if (product == null)
                {
                    _logger.LogInformation($"Id为{id}的产品没有找到");
                    return NotFound();
                }
                _mailService.Send(nameof(ProductController) + "-" + nameof(Index), $"注意：有人查看了Id为{id}的产品");
                return Ok(product);
            }
            catch(Exception ex)
            {
                //log到debug窗口肯定不能用在生产环境，需要使用跨平台的NLog，JSNLog，等，这里推荐NLog
                _logger.LogError($"查找Id为{id}产品出现错误",ex);
                //返回固定的statuscode，然后加一个参数解释，不建议返回ex的具体信息
                return StatusCode(500, "处理请求时发生了错误");
            }
        }
        [HttpPost("add")]
        //[Route("add")]
        //[FromBody]请求的body里面包含着方法需要的实体数据，frombody就是把这个数据序列化成参数model
        public IActionResult Add([FromBody] ProductAddPostModel model)
        {
            //如果客户端发起了一个Bad Request，导致数据不能被序列化，则参数model就会变成null，
            //这是客户端引发的，要让客户端知道，所以返回一个badrequest 400(Bad Request表示客户端引起的错误)的status code
            if (model==null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var maxId = _productSvc.GetAll().Max(t => t.Id);
            var newProduct = new ProductDTO {
                Id = ++maxId,//先自增，再赋值
                Name=model.Name,
                Price=model.Price,
            };
            
            //对于POST, 建议的返回Status Code 是 201 (Created), 可以使用CreatedAtRoute这个内置的Helper Method. 
            //它可以返回一个带有地址Header的Response, 这个Location Header将会包含一个URI, 通过这个URI可以找到我们新创建的实体数据.
            //这里就是指之前写的Index(long id)这个方法. 但是这个Action必须有一个路由的名字才可以引用它, 
            //所以在Index方法上的Route这个attribute里面加上Name ="Index", 然后在CreatedAtRoute方法第一个参数写上这个名字就可以了, 
            //尽管进行了引用, 但是Post方法走完的时候并不会调用Index方法. 
            //CreatedAtRoute第二个参数就是对应着Index的参数列表,使用匿名类即可, 最后一个参数是我们刚刚创建的数据实体.
            return CreatedAtRoute("Index",new { id=newProduct.Id},newProduct);
        }
        //[HttpPost]
        //[Route("delete/{id}")]
        //public IActionResult Delete(long id)
        //{
        //    var product = ProductService.Current.Products.SingleOrDefault(t => t.Id == id);
        //    if(product==null)
        //    {
        //        return NotFound();
        //    }
        //    ProductService.Current.Products.Remove(product);
        //    //假设删除时发送邮件
        //    _localMailService.Send(nameof(ProductController) + "-" + nameof(Delete), $"Id为{id}的产品被删除了");
        //    return NoContent();
        //}
    }
}
