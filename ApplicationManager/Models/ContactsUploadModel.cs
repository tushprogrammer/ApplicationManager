﻿using ApplicationManager_ClassLibrary.Entitys;

namespace ApplicationManager.Models
{
    public class ContactsUploadModel
    {
        public List<Contacts> Contacts { get; set; }
        public List<SocialNet_with_image> SocialNets { get; set; }
    }
}
