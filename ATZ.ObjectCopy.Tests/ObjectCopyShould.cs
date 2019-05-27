using ATZ.ObjectCopy.Tests.TestHelpers;
using FluentAssertions;
using NUnit.Framework;

namespace ATZ.ObjectCopy.Tests
{
    [TestFixture]
    public class ObjectCopyShould
    {
        [Test]
        public void CopyBetweenTwoObjectsCorrectly()
        {
            var o1 = new BaseClass { P1 = 13, P2 = 42 };
            var o2 = new BaseClass();

            o1.ObjectCopyTo(o2);
            o2.P1.Should().Be(13);
            o2.P2.Should().Be(42);
        }

        [Test]
        public void NotCopyReadOnlyProperties()
        {
            var ro1 = new ReadOnlyPropertyClass(13);
            var ro2 = new ReadOnlyPropertyClass(42);

            Assert.DoesNotThrow(() => ro1.ObjectCopyTo(ro2));
            ro2.Ro.Should().Be(42);
        }

        [Test]
        public void BeAbleToCopyBetweenDifferentClasses()
        {
            var o1 = new BaseClass { P1 = 13, P2 = 42 };
            var o2 = new BaseClass2();

            o1.ObjectCopyTo(o2);
            o2.P1.Should().Be(13);
            o2.P2.Should().Be(42);
        }

        [Test]
        public void CopyIntoWriteOnlyProperty()
        {
            var o = new BaseClass { P1 = 13 };
            var wo = new WriteOnlyPropertyClass();

            o.ObjectCopyTo(wo);
            wo.Read.Should().Be(13);
        }

        [Test]
        public void ProvideSolutionForCopyingIntoTheBaseClass()
        {
            var b = new BaseClass { P1 = 13, P2 = 42 };
            var d = new DerivedClass();

            b.ObjectCopyTo(d, atTargetTypeLevel: typeof(BaseClass));
            d.P1.Should().Be(13);
            d.P2.Should().Be(42);
        }

        [Test]
        public void ProvideCapabilityToLimitCopiedInformation()
        {
            var b1 = new BaseClass { P1 = 13, P2 = 42 };
            var b2 = new BaseClass { P1 = 14, P2 = 43 };

            b1.ObjectCopyTo(b2, onlyProperties: new[] { nameof(BaseClass.P2) });
            b2.P1.Should().Be(14);
            b2.P2.Should().Be(42);
        }

        [Test]
        public void CopyBasePropertiesTooWhenNotGivingCopyLevelLimitation()
        {
            var d1 = new DerivedClass { P1 = 42 };
            var d2 = new DerivedClass { P1 = 13 };

            d1.ObjectCopyTo(d2, onlyProperties: new[] { nameof(BaseClass.P1) });
            d2.P1.Should().Be(42);
        }

        [Test]
        public void ShouldNotTryToCopyWriteOnlySourceProperty()
        {
            var s = new NoGetterTestSourceClass();
            var t = new DerivedClass { P3 = 42 };

            s.ObjectCopyTo(t, onlyProperties: new[] { nameof(DerivedClass.P3) });
            t.P3.Should().Be(42);
        }

        [Test]
        public void CopyBaseClassPropertyCorrectly()
        {
            var d1 = new DerivedClass { P1 = 42 };
            var d2 = new DerivedClass();

            d1.ObjectCopyTo(d2);

            d2.P1.Should().Be(d1.P1);
        }

        [Test]
        public void IgnoreIncompatiblePropertyTypes()
        {
            var incompatibleA = new IncompatibleA { P1 = 42 };
            var incompatibleB = new IncompatibleB { P1 = "unset" };
            
            incompatibleA.ObjectCopyTo(incompatibleB);

            incompatibleB.P1.Should().Be("unset");
        }

        [Test]
        public void NotCrashOnPrivateProperties()
        {
            var o1 = new RenamedHiddenPropertyDerived();
            var o2 = new RenamedHiddenPropertyDerived();
            
            Assert.DoesNotThrow(() => o1.ObjectCopyTo(o2));
        }

        [Test]
        public void CopyCollectionsProperly()
        {
            var source = new ObjectWithCollectionProperty<DerivedClass>();
            var target = new ObjectWithCollectionProperty<BaseClass>();
            
            var d = new DerivedClass();
            var b = new BaseClass();
            
            source.Property.Add(d);
            target.Property.Add(b);
            
            source.ObjectCopyTo(target);
            target.Property.Should().ContainInOrder(d).And.HaveCount(1);
        }

        [Test]
        public void NotCopyCollectionsWithIncompatibleTypes()
        {
            var source = new ObjectWithCollectionProperty<BaseClass>();
            var target = new ObjectWithCollectionProperty<DerivedClass>();
            
            var d = new DerivedClass();
            var b = new BaseClass();
            
            source.Property.Add(b);
            target.Property.Add(d);
            
            source.ObjectCopyTo(target);
            target.Property.Should().ContainInOrder(d).And.HaveCount(1);
        }

        [Test]
        public void NotCopyIfSourceCollectionIsNull()
        {
            var o = new BaseClass();
            
            var source = new ObjectWithCollectionProperty<BaseClass>
            {
                Property = null
            };
            var target = new ObjectWithCollectionProperty<BaseClass>
            {
                Property = { o }
            };
            
            Assert.DoesNotThrow(() => source.ObjectCopyTo(target));
            target.Property.Should().ContainInOrder(o).And.HaveCount(1);
        }

        [Test]
        public void NotCopyIfTargetColletionIsNull()
        {
            var o = new BaseClass();
            
            var source = new ObjectWithCollectionProperty<BaseClass>
            {
                Property = { o }
            };
            var target = new ObjectWithCollectionProperty<BaseClass>
            {
                Property = null
            };
            
            Assert.DoesNotThrow(() => source.ObjectCopyTo(target));
        }
    }
}
