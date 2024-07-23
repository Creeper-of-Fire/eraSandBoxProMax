using System;
using System.Collections.Generic;
using eraSandBox.Coitus.XmlAssign;
using eraSandBox.Thought;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.CoitusSimple;

public class PartManager(IHasParts owner) : INeedInitialize
{
    public IHasParts owner = owner;

    public void Initialize()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MessageSpreader> MakeMessageSpreader()
    {
        throw new NotImplementedException();
    }

    public View ProcessMessage(Message message)
    {
        throw new NotImplementedException();
    }
}