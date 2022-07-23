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

            double mockTeachBuffer1Value = 20;
            var mockTeachBuffer1 = new Mock<ITeachEffect>();
            mockTeachBuffer1.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockTeachBuffer1Value);
            mockTeachBuffer1.Setup(effect => effect.name).Returns("TEACH_MOCK1");

            double mockTeachBuffer2Value = 30;
            var mockTeachBuffer2 = new Mock<ITeachEffect>();
            mockTeachBuffer2.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockTeachBuffer2Value);
            mockTeachBuffer2.Setup(effect => effect.name).Returns("TEACH_MOCK2");

            double mockLeanBuffer0Value = 10;
            var mockLeanBuffer0 = new Mock<ILearnEffect>();
            mockLeanBuffer0.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer0Value);
            mockLeanBuffer0.Setup(effect => effect.name).Returns("LEARN_MOCK0");

            double mockLeanBuffer1Value = 20;
            var mockLeanBuffer1 = new Mock<ILearnEffect>();
            mockLeanBuffer1.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer1Value);
            mockLeanBuffer1.Setup(effect => effect.name).Returns("LEARN_MOCK1");

            double mockLeanBuffer2Value = 30;
            var mockLeanBuffer2 = new Mock<ILearnEffect>();
            mockLeanBuffer2.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer2Value);
            mockLeanBuffer2.Setup(effect => effect.name).Returns("LEARN_MOCK2");

            double mockLeanBuffer3Value = 40;
            var mockLeanBuffer3 = new Mock<ILearnEffect>();
            mockLeanBuffer3.Setup(effect => effect.GetEffectValue(currSkill)).Returns(mockLeanBuffer3Value);
            mockLeanBuffer3.Setup(effect => effect.name).Returns("LEARN_MOCK3");

            var teacher0 = Mock.Of<ITeacherLabel>(label=> label.teachEffects == new ITeachEffect[]
            {
                mockTeachBuffer0.Object,
                mockTeachBuffer1.Object
            });

            var teacher1 = Mock.Of<ITeacherLabel>(label => label.teachEffects == new ITeachEffect[]
            {
                mockTeachBuffer2.Object,
            });

            var student00 = Mock.Of<IStudentLabel>(label => label.learnEffects == new ILearnEffect[]
            {
                mockLeanBuffer0.Object
            });

            var student01 = Mock.Of<IStudentLabel>(label => label.learnEffects == new ILearnEffect[]
            {
                mockLeanBuffer1.Object
            });

            var student10 = Mock.Of<IStudentLabel>(label => label.learnEffects == new ILearnEffect[]
            {
                mockLeanBuffer2.Object,
                mockLeanBuffer3.Object
            });

            var spaces = new ITeachLearnSpace[]
            {
                Mock.Of<ITeachLearnSpace>(sp=>
                sp.skillType == SkillType.Elemental
                && sp.teacher == teacher0
                && sp.students == new IStudentLabel[]{
                    student00,
                    student01
                }),
                
                Mock.Of<ITeachLearnSpace>(sp=>
                sp.skillType == SkillType.Elemental
                && sp.teacher == teacher1
                && sp.students == new IStudentLabel[]{
                    student10
                })
            };

            var system = new TeachSystem();
            system.OnTimeLapse(spaces);


            var details = new (string, double)[] {
                ("LEARN_MOCK0", mockLeanBuffer0Value),
                ("Teach", mockTeachBuffer0Value + mockTeachBuffer1Value)
            };

            Mock.Get(student00).Verify(student =>
                student.AddLearn(It.Is<LearnComponent>(
                    learn => learn.skillType == spaces[0].skillType
                    && learn.value == mockTeachBuffer0Value + mockTeachBuffer1Value + mockLeanBuffer0Value
                    && learn.details.SequenceEqual(details))),
                Times.Once());

            details = new (string, double)[] {
                ("LEARN_MOCK1", mockLeanBuffer1Value),
                ("Teach", mockTeachBuffer0Value + mockTeachBuffer1Value)
            };

            Mock.Get(student01).Verify(student =>
                student.AddLearn(It.Is<LearnComponent>(
                    learn => learn.skillType == spaces[0].skillType
                    && learn.value == mockTeachBuffer0Value + mockTeachBuffer1Value + mockLeanBuffer1Value
                    && learn.details.SequenceEqual(details))),
                Times.Once());


            details = new (string, double)[] { 
                ("LEARN_MOCK2", mockLeanBuffer2Value), 
                ("LEARN_MOCK3", mockLeanBuffer3Value), 
                ("Teach", mockTeachBuffer2Value) 
            };

            Mock.Get(student10).Verify(student =>
                student.AddLearn(It.Is<LearnComponent>(
                    learn => learn.skillType == spaces[1].skillType
                    && learn.value == mockTeachBuffer2Value + mockLeanBuffer2Value + mockLeanBuffer3Value
                    && learn.details.SequenceEqual(details))),
                Times.Once());
        }
    }
}