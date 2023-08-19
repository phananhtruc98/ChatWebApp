export default class UtilsService {
  public static isEmpty(array: any): boolean {
    if (!array?.length) {
      return true;
    }
    return false;
  }
}
