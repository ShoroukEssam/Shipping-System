export interface getAllBranch {
  id: number;
  name: string;
  isDeleted: boolean;
  status: boolean;
  stateId: number;
  date: string;
}
export interface Branch {
  id: number;
  name: string;
  stateId: number;
}
