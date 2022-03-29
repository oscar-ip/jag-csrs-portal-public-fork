export interface questionnaireClickData {
  step: number;
  question: string;
  label: string;
  url?: string | null;
}
export interface questionnaireStepData
{
  step: number;
  question: string;
  response?: string | null;
  direction: string;
}


