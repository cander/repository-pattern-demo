namespace Ado.Model.Base;
public abstract class EntityBase
{
    // Note: this assumes that every model has a primary key called Id,
    // which is what the SQL schema created. If you wanted to primary keys
    // with names like EmployeeId and DepartmentId, you probably wouldn't
    // have this attribute here, and the base class might be empty.
    public int Id { get; set; }
}
