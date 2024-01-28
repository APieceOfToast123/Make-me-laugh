namespace _Scripts.State
{
    //所有角色的状态字典
    public enum StateID
    {
        Selectable,
        Settled,
        Laugh,
        Normal,
        Cry,
        Complete,
        fail,
        waiting,
        
        /**===================================医生状态======================================*/
        
        DocSelectable,
        DocTreating
    }
}