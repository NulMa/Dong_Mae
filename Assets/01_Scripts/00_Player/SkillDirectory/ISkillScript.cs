

public interface ISkillScript
{
    public SkillSet GetBind();

    public void Init(Playrer playrer, SkillSet _set);

    public void Process();

    public void Execute();
    public void Release();

}