export interface IRoleWithAllClaims {
  roleId: string;
  roleName: string;
  allRoleCalims: {
    $id: string;
    $values: IClaimsForCheckBox[];
  };
}

  
  export interface IClaimsForCheckBox {
    displayValue: string;
    isSelected: boolean;
    arabicName?: string;
  }
  