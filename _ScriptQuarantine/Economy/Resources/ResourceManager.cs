using System.Collections.Generic;

public class ResourceManager
{
    public List<Resource> resources = new();

    public int Get(ResourceType type)
    {
        foreach (var r in resources)
            if (r.type == type)
                return r.amount;

        return 0;
    }

    public void Add(ResourceType type, int amount)
    {
        foreach (var r in resources)
        {
            if (r.type == type)
            {
                r.amount += amount;
                return;
            }
        }

        resources.Add(new Resource { type = type, amount = amount });
    }

    public void SetResource(ResourceType type, int amount)
    {
        foreach (var r in resources)
        {
            if (r.type == type)
            {
                r.amount = amount;
                return;
            }
        }

        resources.Add(new Resource { type = type, amount = amount });
    }

    public bool Spend(ResourceType type, int amount)
    {
        foreach (var r in resources)
        {
            if (r.type == type)
            {
                if (r.amount < amount)
                    return false;

                r.amount -= amount;
                return true;
            }
        }

        return false;
    }

    public bool HasEnough(ResourceType type, int amount)
    {
        return Get(type) >= amount;
    }
}
