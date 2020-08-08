import { IErrorDef } from './interfaces';

const standartErrors: IErrorDef[] = [
  { error: 'required', localizationKey: 'validation.required' },
  { error: 'minlength', localizationKey: 'validation.minlength', errorProperty: 'requiredLength' },
  {
    error: 'maxlength',
    localizationKey: 'validation.maxlength',
    errorProperty: 'requiredLength',
  },
  { error: 'email', localizationKey: 'validation.email' },
  { error: 'pattern', localizationKey: 'validation.pattern', errorProperty: 'requiredPattern' },
  { error: 'confirm', localizationKey: 'validation.confirm', errorProperty: 'requiredPattern' },
  { error: 'exist', localizationKey: 'validation.exist' },
];

export default standartErrors;
