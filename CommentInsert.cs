﻿using System.Xml.Linq;

namespace RTK
{
    internal static class CommentInsert
    {
        internal static (bool, string) CommentsInsertProcessing(string CurrentFile)
        {
            string error = string.Empty;
            //Позволяет избежать падения при загрузке сломанного .xml файла
            try
            {
                XDocument.Load(CurrentFile, LoadOptions.PreserveWhitespace);
            }
            catch
            {
                error = "Не удалось загрузить файл " + CurrentFile;
                return (false, error);
            }

            XDocument xDoc = XDocument.Load(CurrentFile, LoadOptions.PreserveWhitespace);
            //Позволяет избежать обработки пустого или не содержащего нужных тегов файла
            if (xDoc.Element("LanguageData") is null)
            {
                error = "Не удалось найти LanguageData " + CurrentFile;
                return (false, error);
            }
            //Перевод контекста в содержимое тега LanguageData
            XElement? root = xDoc.Element("LanguageData");
            if (root?.Elements() is null)
            {
                error = "Тег LanguageData пуст " + CurrentFile;
                return (false, error);
            }

            foreach (XElement node in root.Elements())
            {
                //Получение содержимого текущего тега
                string content = node.Value;
                //Создание комментария с ним
                XRaw comment = new("<!-- EN: " + content + " -->\n\t");
                //Добавление этого комментария перед текущим тегом
                node.AddBeforeSelf(comment);
            }
            //Перенос строки перед закрывающим тегом LanguageData
            root.LastNode?.AddAfterSelf("\n");

            //Сохранение файла
            xDoc.Save(CurrentFile);
            return (true, error);
        }
    }
}
