using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Domain
{
    public abstract class MemberVisitor<T>
    {
        public T DynamicVisit(Member p) { return Visit((dynamic)p); }
        protected abstract T Visit(Member p);
        protected virtual T Visit(SimpleMember c) { return Visit((SimpleMember)c); }
        protected virtual T Visit(ComplexMember e) { return Visit((ComplexMember)e); }
        protected virtual T Visit(ValueObjectMember e) { return Visit((ValueObjectMember)e); }
        protected virtual T Visit(FixedLengthTextSimpleMember e) { return Visit((FixedLengthTextSimpleMember)e); }
        protected virtual T Visit(FixedLengthTextComplexMember e) { return Visit((FixedLengthTextComplexMember)e); }
        protected virtual T Visit(DelimitedTextComplexMember e) { return Visit((DelimitedTextComplexMember)e); }
        protected virtual T Visit(DelimitedTextSimpleMember e) { return Visit((DelimitedTextSimpleMember)e); }
        protected virtual T Visit(Column e) { return Visit((Column)e); }
    }
}