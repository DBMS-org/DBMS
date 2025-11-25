declare module 'pdfmake/build/pdfmake' {
  import { TCreatedPdf, TDocumentDefinitions } from 'pdfmake/interfaces';

  const pdfMake: {
    createPdf(documentDefinitions: TDocumentDefinitions): TCreatedPdf;
    vfs?: any;
  };

  export default pdfMake;
}

declare module 'pdfmake/build/vfs_fonts' {
  const vfs: any;
  export default vfs;
}
