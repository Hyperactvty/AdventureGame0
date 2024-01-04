using System;
using System.Collections.Generic;


public class Require_Params
{
  public enum REQUIRE_PARAMS {PlayerLevel,GameLevel};
  // public Dictionary<REQUIRE_PARAMS, Type> dRequireParams => new()
  // {
  //   new(REQUIRE_PARAMS.PlayerLevel, typeof(int)),
  //   new(REQUIRE_PARAMS.GameLevel,typeof(int)),
  // };  
}

public class DroppableSource
{
  public enum SOURCETYPE { Enemy, Chest, Limited_Merchant };
  public string sourceId; // enemyId
  public SOURCETYPE sourceType;
  public float chance; // x/100, x/3000, ect

  public Require_Params requirements;

}
