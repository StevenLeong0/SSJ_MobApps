namespace SeniorLearnApi.Interfaces;

public interface IBulletinTypeListService<T>
{
    //Task<List<T>> GetBulletinListByDate<U>(U Category);
}
/*Explanation
Interface Method that returns a List of Generic type, This IMethod takes 1 parameter. The purpose of <U>(U Type) is so this method can be reused to take on MemberType/MemberBulletinCategory and OfficialType enums. 

*/