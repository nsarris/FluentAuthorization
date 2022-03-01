namespace FluentAuthorization
{
    //public class EntityAccess
    //{
    //    public bool? View { get; }
    //    public bool? Edit { get; }
    //    public bool? Create { get; }
    //    public bool? Delete { get; }
    //}

    //public enum EntityType
    //{
    //    Account,
    //    Concact
    //}

    //public class EntityResource
    //{
    //    public string Name { get; set; }
    //    public Guid Id { get; set; }
    //}

    //public class EntityAccessPolicy : Policy<MyUser, EntityType, EntityAccess>
    //{
    //    public Permission View { get; }
    //    public Permission Edit { get; }
    //    public Permission Create { get; }
    //    public Permission Delete { get; }

    //    public EntityAccessPolicy()
    //    {
    //        this.For(x => x.View).AssertWith(x => x.Data.View);
    //    }
    //}

    //public class RecordAccessPolicy : Policy<MyUser, EntityResource, EntityAccess>
    //{
    //    public override string Key => "asdasda"; 

    //    public Permission View { get; }
    //    public Permission Edit { get; }
    //    public Permission Create { get; }
    //    public Permission Delete { get; }

    //    public RecordAccessPolicy()
    //    {
    //        this.For(x => x.View).AssertWith(x => x.Data.View);
    //    }
    //}

    //public class Test
    //{
    //    public async Task Foo()
    //    {
    //        IPolicyContextProvider<MyUser> contextProvider = new PolicyContextProvider<MyUser>(
    //            userContextProvider: null,
    //            policyProvider: null,
    //            dataProvider: null,
    //            serviceProvider: null
    //            );

    //        var contextBuilder = contextProvider
    //            .ForResource(EntityType.Account)
    //            .ForPolicy<EntityAccessPolicy>();


    //        var ctx = await contextProvider
    //            .ForResource(EntityType.Account)
    //            .ForPolicy<EntityAccessPolicy>()
    //            .BuildContextAsync();

    //        var policy = ctx.Policy;
    //        var data = ctx.GetData();
    //        var result = ctx.Assert(x => x.View)
    //            & ctx.Assert(x => x.Create);

    //        var typedData = contextBuilder.GetAggregatedData();
    //    }
    //}
}
