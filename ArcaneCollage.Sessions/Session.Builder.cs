using ArcaneCollage.Components;
using ArcaneCollage.Entities;
using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace ArcaneCollage.Sessions
{
    public partial class Session
    {
        public static class Builder
        {
            public static Session Build()
            {
                var session = new Session();
                session.time = new Time(1,1,1, 8);

                var entities = new HashSet<IEntity>();
                entities.Add(new Entity(new TeachLearnSpace()));

                var teacher = new Entity();
                var teacherLabel = new TeacherLabel(teacher);
                teacher.AddComponent(teacherLabel);
                entities.Add(teacher);

                var student = new Entity();
                var studentLabel = new StudentLabel(teacher);
                student.AddComponent(studentLabel);
                entities.Add(student);

                session.entities = entities;

                return session;
            }
        }
    }
}
