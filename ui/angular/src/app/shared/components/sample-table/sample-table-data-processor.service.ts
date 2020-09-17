import { Injectable, Injector } from '@angular/core';
import { STColumn } from '@delon/abc/st';
import { ColumnItemDto, ColumnItemFixed, ColumnItemStatistical } from '@service-proxies';
import { SampleComponentBase } from '@shared/common';
import * as _ from 'lodash';
import * as moment from 'moment';


@Injectable()
export class SampleTableDataProcessorService extends SampleComponentBase {

  colTypeMap = {
    datetime: undefined,
    number: undefined,
  };

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  /** 处理列表配置 */
  processCols(cols: ColumnItemDto[]): STColumn[] {
    if (!cols) {
      return [];
    }

    const newCols = cols.sort((a, b) => a.order - b.order);
    const result: STColumn[] = [];
    for (const item of newCols) {
      const newItem: STColumn = {
        index: item.field.split('.'),
        title: this.l(item.title),
        render: item.render,
        width: item.width,
        sort: true,
      };
      if (item.type && item.type !== '' && item.type !== 'yn') {
        newItem.type = item.type.toLowerCase() as any;
        if (newItem.type === 'no'
          || newItem.type === 'checkbox') {
          newItem.sort = undefined;
        } else if (newItem.type === ('action' as any)) {
          newItem.type = undefined;
          newItem.sort = undefined;
          newItem.index = 'actions';
          newItem.render = 'actions';
        }
      }
      if (item.statistical && item.statistical !== ColumnItemStatistical.None) {
        newItem.statistical = item.statistical.toString().toLowerCase() as any;
      }
      if (item.fixed && item.fixed !== ColumnItemFixed.None) {
        newItem.fixed = item.fixed.toString().toLowerCase() as any;
      }
      result.push(newItem);
    }

    return result;
  }

  /** 处理列表数据 */
  processData<T>(data: T[], cols: ColumnItemDto[]): T[] {
    if (!data || !cols) {
      return [];
    }
    const result: T[] = [];

    let actions;
    // 遍历表格数据
    for (const item of data) {
      // 复制一份新数据
      const newItem = _.cloneDeep(item) as any;
      // 设置原始数据
      newItem.original = item;
      result.push(newItem);

      // 遍历列配置
      for (const col of cols) {
        // 字段描述
        const fields = col.field.split('.');

        // 判断类型,跳过字段处理
        if (!col.type
          || col.type === 'checkbox'
          || col.type === 'no') {
          continue;
        }

        if (col.type === 'action') {
          if (!actions) {
            actions = col.actions;
          }
          newItem.actions = actions;
          continue;
        }

        // 获取字段值,如果为空或NaN 则跳过格式化步骤
        let fieldValue = this.getFieldValue(newItem, fields);
        if (isNaN(fieldValue)
          || typeof (fieldValue) === 'undefined'
          || (!fieldValue && fieldValue !== 0 && fieldValue !== false)) {
          continue;
        }

        // 根据类型格式化字段值,并处理根据执行结果决定需要重新给字段设置值
        let needSetValue = false;
        switch (col.type) {
          case 'datetime': // 时间类型
            if (col.dateFormat && col.dateFormat.trim() !== '') {
              if (fieldValue instanceof moment) {
                fieldValue = (fieldValue as moment.Moment).format(col.dateFormat);
              } else {
                fieldValue = moment(fieldValue).format(col.dateFormat);
              }
              needSetValue = true;
            }
            break;
          case 'number': // number类型列，处理小数点
            if (typeof (fieldValue) === 'number') {
              fieldValue = fieldValue.toFixed(2);
            } else {
              fieldValue = parseFloat(fieldValue).toFixed(2);
            }
            needSetValue = true;
            break;
          case 'yn':
            debugger;
            if (typeof (fieldValue) === 'boolean') {
              fieldValue = fieldValue ? this.l('label.yes') : this.l('label.no');
              needSetValue = true;
            }
            break;
        }

        // 更新字段值
        if (needSetValue) {
          this.setFieldValue(newItem, fields, fieldValue);
        }
      }
    }

    return result;
  }

  /** 获取字段值 */
  private getFieldValue(data: any, fields: string | string[]): any {
    if (!data) {
      console.debug('getFieldValue 数据为空');
      return undefined;
    }
    if (!fields) {
      console.debug('getFieldValue 列表配置字段为空');
      return undefined;
    }
    if (!Array.isArray(fields)) {
      return data[fields];
    }
    let resultData = data;
    for (const field of fields) {
      resultData = this.getFieldValue(resultData, field);
      if (!data) {
        return undefined;
      }
    }
    return resultData;
  }

  /** 给字段设置值 */
  private setFieldValue(data: any, fields: string | string[], val: any): boolean {
    if (!data) {
      console.debug('setFieldValue 数据为空');
      return undefined;
    }
    if (!fields) {
      console.debug('setFieldValue 列表配置字段为空');
      return undefined;
    }

    if (!Array.isArray(fields)) {
      data[fields] = val;
      return false;
    }

    if (fields.length === 1) {
      this.setFieldValue(data, fields[0], val);
      return false;
    }


    const max = fields.length - 1;
    const lastField = fields[max];
    let resultData = data;

    for (let i = 0; i < max; i++) {
      if (!resultData) {
        console.debug(`给字段设置值失败，调用链存在空数据，异常调用链: ${fields[i - 1]}  原始调用链：`);
        console.debug(fields);
        return false;
      }
      const field = fields[i];
      resultData = resultData[field];
    }

    resultData[lastField] = val;
    return true;
  }
}
