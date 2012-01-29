﻿using System.Linq;
using System.Xml.Linq;
using Should;
using Xunit;

namespace Cassette
{
    public class BundleManifestWriter_Tests
    {
        readonly TestableBundleManifest manifest;
        XElement element;

        class TestableBundleManifest : BundleManifest
        {
            protected override Bundle CreateBundleCore()
            {
                throw new System.NotImplementedException();
            }
        }

        public BundleManifestWriter_Tests()
        {
            manifest = new TestableBundleManifest
            {
                Path = "~",
                Hash = new byte[] { 1, 2, 3 },
                ContentType = "content-type",
                PageLocation = "page-location",
                Assets =
                    {
                        new AssetManifest
                        {
                            Path = "~/asset",
                            RawFileReferences =
                                {
                                    "~/raw-file/reference"
                                }
                        }
                    },
                References =
                    {
                        "~/bundle-reference"
                    }
            };

            WriteToElement();
        }

        void WriteToElement()
        {
            var container = new XDocument();
            var writer = new BundleManifestWriter<TestableBundleManifest>(container);
            writer.Write(manifest);
            element = container.Root;
        }

        [Fact]
        public void PathAttributeEqualsManifestPath()
        {
            element.Attribute("Path").Value.ShouldEqual(manifest.Path);
        }

        [Fact]
        public void HashAttributeEqualsHexStringOfManifestHash()
        {
            element.Attribute("Hash").Value.ShouldEqual("010203");
        }

        [Fact]
        public void ContentTypeAttributeEqualsManifestContentType()
        {
            element.Attribute("ContentType").Value.ShouldEqual(manifest.ContentType);
        }

        [Fact]
        public void PageLocationAttributeEqualsManifestPageLocation()
        {
            element.Attribute("PageLocation").Value.ShouldEqual(manifest.PageLocation);
        }

        [Fact]
        public void ElementHasAssetChildElement()
        {
            element.Elements("Asset").Count().ShouldEqual(1);
        }

        [Fact]
        public void ElementHasReferenceChildElement()
        {
            element.Elements("Reference").Count().ShouldEqual(1);
        }

        [Fact]
        public void GivenContentTypeNullThenElementHasNoContentTypeAttribute()
        {
            manifest.ContentType = null;
            WriteToElement();
            element.Attribute("ContentType").ShouldBeNull();
        }

        [Fact]
        public void GivenPageLocationNullThenElementHasNoPageLocationAttribute()
        {
            manifest.PageLocation = null;
            WriteToElement();
            element.Attribute("PageLocation").ShouldBeNull();
        }
    }
}
