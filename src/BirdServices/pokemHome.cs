using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using DotnetSpider;
using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Parser.Formatters;
using DotnetSpider.DataFlow.Storage.Entity;
using DotnetSpider.Http;
using DotnetSpider.Selector;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace BirdServices;
record skil(string No, string Name, string JpName, string EnName, string property, string type, string harm, string hit,
    string pp, string descript,int era);
[DisplayName("博客园爬虫")]
public class PokemonHomeSpider: Spider
{
    public PokemonHomeSpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
    {
    }

    protected override async Task InitializeAsync(CancellationToken stoppingToken = new CancellationToken())
    {
        //AddDataFlow<ListNewsParser>();
        //AddDataFlow<NewsParser>();
        // AddDataFlow(GetDefaultStorage);
        AddDataFlow<DataParser<CnblogsEntry>>();
        var request = new Request("https://news.cnblogs.com/n/page/1",new Dictionary<string, object> { { "网站", "博客园" } });
        request.Headers.UserAgent = "";

        await AddRequestsAsync(request);
    } 
    
    [Schema("cnblogs", "news")]
    [EntitySelector(Expression = ".//div[@class='news_block']", Type = SelectorType.XPath)]
    [GlobalValueSelector(Expression = ".//a[@class='current']", Name = "类别", Type = SelectorType.XPath)]
    [GlobalValueSelector(Expression = "//title", Name = "Title", Type = SelectorType.XPath)]
    [FollowRequestSelector(SelectorType= SelectorType.XPath, Expressions = new []{"//div[@class='pager']"})]
    public class CnblogsEntry : EntityBase<CnblogsEntry>
    {
        protected override void Configure()
        {
            HasIndex(x => x.Title);
            HasIndex(x => new { x.WebSite, x.Guid }, true);
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [ValueSelector(Expression = "类别", Type = SelectorType.Environment)]
        public string Category { get; set; }

        [Required]
        [StringLength(200)]
        [ValueSelector(Expression = "网站", Type = SelectorType.Environment)]
        public string WebSite { get; set; }

        [StringLength(200)]
        [ValueSelector(Expression = "Title", Type = SelectorType.Environment)]
        [ReplaceFormatter(NewValue = "", OldValue = " - 博客园")]
        public string Title { get; set; }

        [StringLength(40)]
        [ValueSelector(Expression = "GUID", Type = SelectorType.Environment)]
        public string Guid { get; set; }

        [ValueSelector(Expression = ".//h2[@class='news_entry']/a")]
        public string News { get; set; }

        [ValueSelector(Expression = ".//h2[@class='news_entry']/a/@href")]
        public string Url { get; set; }

        [ValueSelector(Expression = ".//div[@class='entry_summary']")]
        [TrimFormatter]
        public string PlainText { get; set; }

        [ValueSelector(Expression = "DATETIME", Type = SelectorType.Environment)]
        public DateTime CreationTime { get; set; }
    }


}

class ListNewsParser : DataParser
{
    public override Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    protected override Task ParseAsync(DataFlowContext context)
    {
        throw new NotImplementedException();
    }
}