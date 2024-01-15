using System.ComponentModel;

namespace EducationTech.Enums
{
    public enum Role
    {
        [Description("Admin")]
        Admin,
        [Description("Group Admin")]
        GroupAdmin,
        [Description("Instructor")]
        Instructor,
        [Description("Learner")]
        Learner
    }
}
