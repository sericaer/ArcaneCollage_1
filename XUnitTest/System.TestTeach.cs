using ArcaneCollage.Components;
using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Effects.Interfaces;
using ArcaneCollage.Entities.Interfaces;
using ArcaneCollage.Skills.Interfaces;
using ArcaneCollage.System;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTest.System
{
    public class TestTeach
    {
        [Fact]
        public void TestTeachNormal ()
        {
            var currSkill = SkillType.Elemental;

            double mockTeachBuffer0Value = 10;
            var mockTeachBuffer0 = new Mock<ITeachEffect>();
            mockTeachBuffer0.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockTeachBuffer0Value);
            mockTeachBuffer0.Setup(effect => effect.name).Returns("TEACH_MOCK0");
            mockTeachBuffer0.As<IComponent>();

            double mockTeachBuffer1Value = 20;
            var mockTeachBuffer1 = new Mock<ITeachEffect>();
            mockTeachBuffer1.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockTeachBuffer1Value);
            mockTeachBuffer1.Setup(effect => effect.name).Returns("TEACH_MOCK1");
            mockTeachBuffer1.As<IComponent>();

            double mockTeachBuffer2Value = 30;
            var mockTeachBuffer2 = new Mock<ITeachEffect>();
            mockTeachBuffer2.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockTeachBuffer2Value);
            mockTeachBuffer2.Setup(effect => effect.name).Returns("TEACH_MOCK2");
            mockTeachBuffer2.As<IComponent>();

            double mockLeanBuffer0Value = 10;
            var mockLeanBuffer0 = new Mock<ILearnEffect>();
            mockLeanBuffer0.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer0Value);
            mockLeanBuffer0.Setup(effect => effect.name).Returns("LEARN_MOCK0");
            mockLeanBuffer0.As<IComponent>();

            double mockLeanBuffer1Value = 20;
            var mockLeanBuffer1 = new Mock<ILearnEffect>();
            mockLeanBuffer1.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer1Value);
            mockLeanBuffer1.Setup(effect => effect.name).Returns("LEARN_MOCK1");
            mockLeanBuffer1.As<IComponent>();

            double mockLeanBuffer2Value = 30;
            var mockLeanBuffer2 = new Mock<ILearnEffect>();
            mockLeanBuffer2.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer2Value);
            mockLeanBuffer2.Setup(effect => effect.name).Returns("LEARN_MOCK2");
            mockLeanBuffer2.As<IComponent>();

            double mockLeanBuffer3Value = 40;
            var mockLeanBuffer3 = new Mock<ILearnEffect>();
            mockLeanBuffer3.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer3Value);
            mockLeanBuffer3.Setup(effect => effect.name).Returns("LEARN_MOCK3");
            mockLeanBuffer3.As<IComponent>();

            var teacher0 = Mock.Of<IEntity>(teacher=>teacher.components == new IComponent[]
            {
                (IComponent)mockTeachBuffer0.Object,
                (IComponent)mockTeachBuffer1.Object
            });

            var teacher1 = Mock.Of<IEntity>(teacher => teacher.components == new IComponent[]
            {
                (IComponent)mockTeachBuffer2.Object,
            });

            var student00 = Mock.Of<IEntity>(teacher => teacher.components == new IComponent[]
            {
                (IComponent)mockLeanBuffer0.Object
            });

            var student01 = Mock.Of<IEntity>(teacher => teacher.components == new IComponent[]
            {
                (IComponent)mockLeanBuffer1.Object
            });

            var student10 = Mock.Of<IEntity>(teacher => teacher.components == new IComponent[]
            {
                (IComponent)mockLeanBuffer2.Object,
                (IComponent)mockLeanBuffer3.Object
            });

            var spaces = new ITeachLearnSpace[]
            {
                Mock.Of<ITeachLearnSpace>(sp=>
                sp.skillType == SkillType.Elemental
                && sp.teacher == teacher0
                && sp.students == new IEntity[]{
                    student00,
                    student01
                }),
                
                Mock.Of<ITeachLearnSpace>(sp=>
                sp.skillType == SkillType.Elemental
                && sp.teacher == teacher1
                && sp.students == new IEntity[]{
                    student10
                })
            };

            var system = new TeachSystem();
            system.OnTimeLapse(spaces);

            var details = new (string, double)[] {
                ("TEACH_MOCK0", mockTeachBuffer0Value),
                ("TEACH_MOCK1", mockTeachBuffer1Value)
            };

            Mock.Get(teacher0).Verify(teacher => 
                teacher.AddComponent(It.Is<TeachComponent>(
                    teach => teach.skillType == spaces[0].skillType 
                    && teach.value == mockTeachBuffer0Value + mockTeachBuffer1Value
                    && teach.details.SequenceEqual(details))), 
                Times.Once());

            Mock.Get(teacher0).Verify(teacher => 
                teacher.RemoveComponent(It.Is<TeachComponent>(
                    teach => teach.skillType == spaces[0].skillType)), 
                Times.Once());

            details = new (string, double)[] {
                ("LEARN_MOCK0", mockLeanBuffer0Value),
                ("Teach", mockTeachBuffer0Value + mockTeachBuffer1Value)
            };

            Mock.Get(student00).Verify(student =>
                student.AddComponent(It.Is<LearnComponent>(
                    learn => learn.skillType == spaces[0].skillType
                    && learn.value == mockTeachBuffer0Value + mockTeachBuffer1Value + mockLeanBuffer0Value
                    && learn.details.SequenceEqual(details))),
                Times.Once());

            details = new (string, double)[] {
                ("LEARN_MOCK1", mockLeanBuffer1Value),
                ("Teach", mockTeachBuffer0Value + mockTeachBuffer1Value)
            };

            Mock.Get(student01).Verify(student =>
                student.AddComponent(It.Is<LearnComponent>(
                    learn => learn.skillType == spaces[0].skillType
                    && learn.value == mockTeachBuffer0Value + mockTeachBuffer1Value + mockLeanBuffer1Value
                    && learn.details.SequenceEqual(details))),
                Times.Once());

            details = new (string, double)[] {
                ("TEACH_MOCK2", mockTeachBuffer2Value)
            };

            Mock.Get(teacher1).Verify(teacher =>
                teacher.AddComponent(It.Is<TeachComponent>(
                    teach => teach.skillType == spaces[1].skillType
                    && teach.value == mockTeachBuffer2Value
                    && teach.details.SequenceEqual(details))),
                Times.Once());

            Mock.Get(teacher1).Verify(teacher =>
                teacher.RemoveComponent(It.Is<TeachComponent>(
                    teach => teach.skillType == spaces[1].skillType)),
                Times.Once());

            details = new (string, double)[] { 
                ("LEARN_MOCK2", mockLeanBuffer2Value), 
                ("LEARN_MOCK3", mockLeanBuffer3Value), 
                ("Teach", mockTeachBuffer2Value) 
            };

            Mock.Get(student10).Verify(student =>
                student.AddComponent(It.Is<LearnComponent>(
                    learn => learn.skillType == spaces[1].skillType
                    && learn.value == mockTeachBuffer2Value + mockLeanBuffer2Value + mockLeanBuffer3Value
                    && learn.details.SequenceEqual(details))),
                Times.Once());
        }

        private class MockTechLearnSpace : ITeachLearnSpace
        {
            public SkillType skillType { get; set; }

            public IEntity teacher { get; set; }

            public IEnumerable<IEntity> students { get; set; }

            public IEnumerable<IComponent> components => throw new NotImplementedException();

            public void AddComponent(IComponent component)
            {
                throw new NotImplementedException();
            }

            public void RemoveComponent(IComponent component)
            {
                throw new NotImplementedException();
            }
        }
    }
}