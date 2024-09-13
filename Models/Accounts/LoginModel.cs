﻿using InventorySystem. Attributes;
using InventorySystem. Utilities;
using System. ComponentModel;
using System. ComponentModel. DataAnnotations;
using System. Text. Json. Serialization;

namespace InventorySystem. Models. Accounts
 {
 public class LoginModel
  {
  [DisplayName(DisplayNames. UserId)]
  public int UserID { get; set; }

  [ValidateField]
  //[NoSpaces]
  [CharacterLength(3, 64)]
  [DisplayName(DisplayNames. Username)]
  [JsonPropertyName("username")]
  //[ValidateUsernameOrEmail]
  public string? Username { get; set; }


  [ValidateField]
  //[NoSpaces]
  [CharacterLength(8, 128)]
  [DisplayName(DisplayNames. Password)]
  [JsonPropertyName("password")]
  [DataType(DataType. Password)]
  public string? Password { get; set; }
  }

 }


