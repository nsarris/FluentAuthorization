using FluentAuthorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authorization
{


    
   
    

    [PolicyId(PolicyEnum.ValueProcessing)]

    public class ValueProcessing : MyPolicy<ValueProcessing.ValueProcessingData>
    {
        public class ValueProcessingData : PolicyData
        {
            public bool AllowView = true;
            //bool allowAdd = false;
            //bool allowUpdate = false;
            //bool allowDelete = false;
        }


        [PermissionName("AllowView")]
        [PermissionDenialMessage("Permission AllowView of Policy VV not set")]
        public class AllowViewPermission : Permission
        {
            public AllowViewPermission(SecurityPolicy<ValueProcessingData, MyUserSecurityContext> policy) : base(policy)
            {
            }

            protected override bool AssertInternal(ValueProcessingData data, MyUserSecurityContext user)
            {
                return data.AllowView;
            }
        }

        public AllowViewPermission AllowView { get; private set; } 


        public IPermission AllowView2 { get; private set; }
            

        [PermissionName("AllowView")]
        [PermissionDenialMessage("Permission AllowView of Policy VV not set")]
        public IPermission AllowView3 { get; private set; }

        public IPermission AllowView4 { get; private set; }
        public IPermission AllowView5 { get; private set; }

        public IAssertable CombinedPermission(UserSecuritySchema<MyUserSecurityContext> policyProvider)
        {
            return policyProvider.WhenAll(a => a.Has((AccessData p) => p.Allow));
        }

        //public IAssertable CombinedPermission2(UserSecuritySchema<MyUserSecurityContext> policyProvider, ValueProcessingData valueProcessingData)
        //{
        //    return policyProvider.WhenAll(a => a.Has((AccessData p) => p.Allow));
        //}

        public ValueProcessing(ValueProcessingData data) : base(data)
        {
            AllowView = new AllowViewPermission(this);
            //AllowView2 = new GenericPermission(this, "AllowView2", u => Data.allowView);

            var permissionBuilder = new PermissionBuilderFactory<ValueProcessing>(this);

            permissionBuilder
                .For(x => x.AllowView3)
                .Assert((d, u) => d.AllowView)
                .BuildAndSet();

            permissionBuilder
                .For(x => x.AllowView3)
                .BuildAndSetLazy(
                    b => b
                    .Assert((p,u) => p.AllowView)
                    .WithName("Test")
                    //.WithMessageBuilder((p,u) => p.Name)
                 );

            AllowView4 = new LazyPermission(this,
                () => permissionBuilder
                .For(x => x.AllowView4)
                .Assert((d,u) => d.AllowView)
                .Build());

            AllowView5 
                = new LazyPermission(this,
                "AllowView5",
                (d,u) => d.AllowView);
        }
    }

    public class AccessDataData : PolicyData
    {
        public bool Allow { get; set; } = false;
    }

    [PolicyId(PolicyEnum.AccessData)]
    public class AccessData : MyPolicy<AccessDataData>
    {
        //bool allow { get; set; } = false;
        

        public class AllowPermission : Permission
        {
            public AllowPermission(SecurityPolicy<AccessDataData, MyUserSecurityContext> policy) : base(policy)
            {
            }

            protected override bool AssertInternal(AccessDataData data, MyUserSecurityContext user)
            {
                return data.Allow;
            }
        }

        public AllowPermission Allow { get; private set; } //= new AllowPermission();

        public AccessData(AccessDataData data) : base(data)
        {
            Allow = new AllowPermission(this);
        }
    }


    public class CombinedPolicy : MyPolicy
    {
        public IAssertable Allow(UserSecuritySchema<MyUserSecurityContext> policyProvider)
        {
            return policyProvider
                .WhenAll(a => a
                .Has((ValueProcessing x) => x.AllowView)
                .Has(x => x
                    .WhenAny(a1 => a1
                        .Has((AccessData y) => y.Allow))
                     )
                .WithError(() => "Error1 Inner")
                )
                .AndAny(a => a
                    .Has((AccessData x) => x.Allow)
                    .WithError(() => "Error2 Inner")
                    );
        }
    }
}
