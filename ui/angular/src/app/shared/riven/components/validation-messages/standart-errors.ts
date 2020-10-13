import { IErrorDef } from './interfaces';

const standartErrors: IErrorDef[] = [
  { error: 'required', localizationKey: 'validation.required' },
  { error: 'email', localizationKey: 'validation.email' },
  { error: 'confirm', localizationKey: 'validation.confirm' },
  { error: 'exist', localizationKey: 'validation.exist' },
  { error: 'minlength', localizationKey: 'validation.minlength', errorProperty: 'requiredLength' },
  { error: 'maxlength', localizationKey: 'validation.maxlength', errorProperty: 'requiredLength' },
  { error: 'pattern', localizationKey: 'validation.pattern', errorProperty: 'requiredPattern' },
];

export default standartErrors;
