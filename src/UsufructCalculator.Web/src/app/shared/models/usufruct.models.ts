export enum Gender {
  Male = 0,
  Female = 1,
}

export enum CalculationMethod {
  EenLeven = 0,
}

export interface UsufructRequest {
  amount: number;
  age: number;
  gender: Gender;
  calculationMethod: CalculationMethod;
}

export interface UsufructResponse {
  originalValue: number;
  factor: number;
  calculatedValue: number;
}
