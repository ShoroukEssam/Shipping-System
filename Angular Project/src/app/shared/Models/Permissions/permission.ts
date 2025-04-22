export interface IPermission {
    id: string;
    roleName: string;
    date: string;
  }
  export interface IPermissionResponse {
    $id:number
    $values: IPermission[]
  }