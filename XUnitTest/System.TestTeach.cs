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

            double mockLeanBuffer0Value = 30;
            var mockLeanBuffer0 = new Mock<ILearnEffect>();
            mockLeanBuffer0.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer0Value);
            mockLeanBuffer0.Setup(effect => effect.name).Returns("LEARN_MOCK1");
            mockLeanBuffer0.As<IComponent>();

            double mockLeanBuffer1Value = 40;
            var mockLeanBuffer1 = new Mock<ILearnEffect>();
            mockLeanBuffer1.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer1Value);
            mockLeanBuffer1.Setup(effect => effect.name).Returns("LEARN_MOCK2");
            mockLeanBuffer1.As<IComponent>();


            var teacher0 = Mock.Of<IEntity>(teacher=>teacher.components == new IComponent[]
            {
                (IComponent)mockTeachBuffer0.Object,
                (IComponent)mockTeachBuffer1.Object
            });

            var student00 = Mock.Of<IEntity>(teacher => teacher.components == new IComponent[]
            {
                (IComponent)mockLeanBuffer0.Object
            });

            var student01 = Mock.Of<IEntity>(teacher => teacher.components == new IComponent[]
            {
                (IComponent)mockLeanBuffer1.Object
            });

            var spaces = new ITeachLearnSpace[]
            {
                new MockTechLearnSpace()
                {
                    skillType = currSkill,
                    teacher = teacher0,
                    students = new IEntity[]
                    {
                        student00,
                        student01
                    }
                }
            };

            var system = new TeachSystem();
            system.OnTimeLapse(spaces);

            Mock.Get(teacher0).Verify(teacher => 
                teacher.AddComponent(It.Is<TeachComponent>(
                    teach => teach.skillType == spaces[0].skillType 
                    && teach.value == mockTeachBuffer0Value + mockTeachBuffer1Value)), 
                Times.Once());

            Mock.Get(teacher0).Verify(teacher => 
                teacher.RemoveComponent(It.Is<TeachComponent>(
                    teach => teach.skillType == spaces[0].skillType)), 
                Times.Once());

            Mock.Get(student00).Verify(student =>
                student.AddComponent(It.Is<LearnComponent>(
                    learn => learn.skillType == spaces[0].skillType
                    && learn.value == mockTeachBuffer0Value + mockTeachBuffer1Value + mockLeanBuffer0Value)),
                Times.Once());

            Mock.Get(student01).Verify(student =>
                student.AddComponent(It.Is<LearnComponent>(
                    learn => learn.skillType == spaces[0].skillType
                    && learn.value == mockTeachBuffer0Value + mockTeachBuffer1Value + mockLeanBuffer1Value)),
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