﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Netcode;
using StardustCore.Objects;

namespace StardustCore.NetCode.Objects
{
    class NetMultiTileObject : Netcode.NetField<MultiTileObject, NetMultiTileObject>
    {
        public NetMultiTileObject()
        {

        }

        public NetMultiTileObject(MultiTileObject obj): base(obj)
        {

        }

        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {
            NetCoreObject obj = new NetCoreObject();
            obj.Read(reader, version);
            //Values already taken care of in NetCoreObject

            NetList<KeyValuePair<Vector2, MultiTileComponent>, NetKeyValuePair<Vector2, MultiTileComponent, NetVector2, NetMultiTileComponent>> netList = new NetList<KeyValuePair<Vector2, MultiTileComponent>, NetKeyValuePair<Vector2, MultiTileComponent, NetVector2, NetMultiTileComponent>>();
            netList.Read(reader, version);
            Value.objects = netList.ToList();

            NetColor col = new NetColor();
            col.Read(reader, version);
            Value.categoryColor = col.Value;

            NetString name = new NetString();
            name.Read(reader, version);
            Value.categoryName = name.Value;
        }

        protected override void WriteDelta(BinaryWriter writer)
        {
            NetCoreObject obj = new NetCoreObject(Value);
            obj.Write(writer);

            NetList<KeyValuePair<Vector2, MultiTileComponent>, NetKeyValuePair<Vector2, MultiTileComponent, NetVector2, NetMultiTileComponent>> netList = new NetList<KeyValuePair<Vector2, MultiTileComponent>, NetKeyValuePair<Vector2, MultiTileComponent, NetVector2, NetMultiTileComponent>>();
            foreach (var v in Value.objects)
            {
                netList.Add(v);
            }
            netList.Write(writer);

            NetColor col = new NetColor(Value.categoryColor);
            col.Write(writer);

            NetString catName = new NetString(Value.categoryName);
            catName.Write(writer);
        }
    }
}
