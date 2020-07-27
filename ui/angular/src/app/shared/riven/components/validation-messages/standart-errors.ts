import { IErrorDef } from './interfaces';

const standartErrors: IErrorDef[] = [
  { error: 'required', localizationKey: 'ThisFieldIsRequired' },
  { error: 'minlength', localizationKey: 'PleaseEnterAtLeastNCharacter', errorProperty: 'requiredLength' },
  {
    error: 'maxlength',
    localizationKey: 'PleaseEnterNoMoreThanNCharacter',
    errorProperty: 'requiredLength',
  },
  { error: 'email', localizationKey: 'InvalidEmailAddress' },
  { error: 'pattern', localizationKey: 'InvalidPattern', errorProperty: 'requiredPattern' },
  { error: 'exist', localizationKey: 'DuplicateData' },
];

export default standartErrors;
